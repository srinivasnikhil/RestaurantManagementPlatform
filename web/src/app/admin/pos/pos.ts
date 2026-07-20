import { Component, inject, signal, computed, OnInit, AfterViewChecked } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { MenuApi } from '../../core/menu-api';
import { OrderService } from '../../core/order';
import { PaymentService, PaymentOrderRequest } from '../../core/payment';
import { PAYPAL_CLIENT_ID } from '../../core/api.config';
import { MenuItem } from '../../core/models';
import { downloadBlob } from '../../core/download';

declare const paypal: any;

interface PosLine { menuItemId: number; name: string; price: number; quantity: number; }

@Component({
  selector: 'app-pos',
  imports: [CurrencyPipe, FormsModule],
  templateUrl: './pos.html',
  styleUrl: './pos.css',
})
export class Pos implements OnInit, AfterViewChecked {
  private menuApi = inject(MenuApi);
  private orders = inject(OrderService);
  private payment = inject(PaymentService);

  items = signal<MenuItem[]>([]);
  lines = signal<PosLine[]>([]);
  orderType = signal<'DineIn' | 'Takeaway'>('DineIn');
  label = signal('');
  message = signal<string | null>(null);
  error = signal<string | null>(null);
  lastOrder = signal<{ id: number; code: string } | null>(null);

  subtotal = computed(() => this.lines().reduce((s, l) => s + l.price * l.quantity, 0));

  private buttonsRendered = false;

  ngOnInit(): void {
    this.menuApi.getMenuItems().subscribe({ next: (d) => this.items.set(d.filter((i) => i.isAvailable)) });
    this.loadSdk().catch(() => this.error.set('Could not load PayPal.'));
  }

  // render the buttons once there are items to pay for
  ngAfterViewChecked(): void {
    if (!this.buttonsRendered && this.lines().length > 0 && typeof paypal !== 'undefined'
        && document.getElementById('pos-paypal')) {
      this.renderButtons();
    }
  }

  private loadSdk(): Promise<void> {
    if (typeof paypal !== 'undefined') return Promise.resolve();
    return new Promise((resolve, reject) => {
      const existing = document.getElementById('paypal-sdk');
      if (existing) { existing.addEventListener('load', () => resolve()); return; }
      const s = document.createElement('script');
      s.id = 'paypal-sdk';
      s.src = `https://www.paypal.com/sdk/js?client-id=${PAYPAL_CLIENT_ID}&currency=USD`;
      s.onload = () => resolve();
      s.onerror = () => reject();
      document.body.appendChild(s);
    });
  }

  addItem(item: MenuItem): void {
    this.lines.update((lines) => {
      const existing = lines.find((l) => l.menuItemId === item.id);
      if (existing) return lines.map((l) => l.menuItemId === item.id ? { ...l, quantity: l.quantity + 1 } : l);
      return [...lines, { menuItemId: item.id, name: item.name, price: item.price, quantity: 1 }];
    });
  }

  changeQty(menuItemId: number, delta: number): void {
    this.lines.update((lines) =>
      lines.map((l) => l.menuItemId === menuItemId ? { ...l, quantity: l.quantity + delta } : l)
           .filter((l) => l.quantity > 0)
    );

    if (this.lines().length === 0) {
      this.buttonsRendered = false;
    }
  }

  private buildRequest(): PaymentOrderRequest {
    return {
      type: this.orderType(),
      customerName: this.label().trim() || (this.orderType() === 'DineIn' ? 'Table' : 'Takeaway'),
      customerPhone: 'Staff-POS',
      items: this.lines().map((l) => ({ menuItemId: l.menuItemId, quantity: l.quantity })),
    };
  }

  private renderButtons(): void {
    this.buttonsRendered = true;
    paypal.Buttons({
      onClick: (_d: any, actions: any) => {
        if (this.lines().length === 0) return actions.reject();
        return actions.resolve();
      },
      createOrder: () => {
        this.error.set(null);
        return firstValueFrom(this.payment.createStaffOrder(this.buildRequest())).then((r) => r.paypalOrderId);
      },
      onApprove: (data: any) => {
        return firstValueFrom(this.payment.captureStaff(data.orderID, this.buildRequest()))
          .then((order) => {
            this.message.set(`Paid. Order #${order.id} sent to the kitchen.`);
            this.lastOrder.set({ id: order.id, code: order.trackingCode });
            this.lines.set([]);
            this.label.set('');
          })
          .catch(() => this.error.set('Payment captured but order failed. Check before retrying.'));
      },
      onError: () => this.error.set('Payment failed. Please try again.'),
    }).render('#pos-paypal');
  }

  downloadReceipt(): void {
    const last = this.lastOrder();
    if (!last) return;
    this.orders.downloadReceiptById(last.id).subscribe({ next: (blob) => downloadBlob(blob, `receipt-order-${last.id}.pdf`) });
  }

  openPrintReceipt(): void {
    const last = this.lastOrder();
    if (last) window.open(`/receipt/${last.code}`, '_blank');
  }
}