import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuApi } from '../../core/menu-api';
import { OrderService } from '../../core/order';
import { MenuItem } from '../../core/models';
import { downloadBlob } from '../../core/download';

interface PosLine { menuItemId: number; name: string; price: number; quantity: number; }

@Component({
  selector: 'app-pos',
  imports: [CurrencyPipe, FormsModule],
  templateUrl: './pos.html',
  styleUrl: './pos.css',
})
export class Pos implements OnInit {
  private menuApi = inject(MenuApi);
  private orders = inject(OrderService);

  items = signal<MenuItem[]>([]);
  lines = signal<PosLine[]>([]);
  orderType = signal<'DineIn' | 'Takeaway'>('DineIn');
  label = signal('');            // table number or customer name
  placing = signal(false);
  message = signal<string | null>(null);
  lastOrder = signal<{ id: number; code: string } | null>(null);

  subtotal = computed(() => this.lines().reduce((s, l) => s + l.price * l.quantity, 0));

  ngOnInit(): void {
    this.menuApi.getMenuItems().subscribe({ next: (d) => this.items.set(d.filter((i) => i.isAvailable)) });
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
      lines
        .map((l) => l.menuItemId === menuItemId ? { ...l, quantity: l.quantity + delta } : l)
        .filter((l) => l.quantity > 0)
    );
  }

  submit(): void {
    if (this.lines().length === 0) return;
    this.placing.set(true);
    this.message.set(null);

    this.orders.placeStaffOrder({
      type: this.orderType(),
      customerName: this.label().trim() || (this.orderType() === 'DineIn' ? 'Table' : 'Takeaway'),
      customerPhone: 'Staff-POS',
      items: this.lines().map((l) => ({ menuItemId: l.menuItemId, quantity: l.quantity })),
    }).subscribe({
      next: (order) => {
        this.message.set(`Order #${order.id} sent to the kitchen.`);
        this.lastOrder.set({ id: order.id, code: order.trackingCode });
        this.lines.set([]);
        this.label.set('');
        this.placing.set(false);
      },
      error: (err) => { this.message.set(err.error ?? 'Could not place the order.'); this.placing.set(false); },
    });
  }

  downloadReceipt(): void {
  const last = this.lastOrder();
  if (!last) return;
  this.orders.downloadReceiptById(last.id).subscribe({
    next: (blob) => downloadBlob(blob, `receipt-order-${last.id}.pdf`),
  });
}

  openPrintReceipt(): void {
    const last = this.lastOrder();
    if (last) window.open(`/receipt/${last.code}`, '_blank');
  }
}