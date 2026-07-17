import { Component, inject, signal, OnInit, AfterViewInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { CartService } from '../core/cart';
import { PaymentService, PaymentOrderRequest } from '../core/payment';
import { PAYPAL_CLIENT_ID } from '../core/api.config';

declare const paypal: any;

@Component({
  selector: 'app-checkout',
  imports: [CurrencyPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit, AfterViewInit {
  protected cart = inject(CartService);
  private payment = inject(PaymentService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  error = signal<string | null>(null);
  message = signal<string | null>(null);
  private buttonsRendered = false;

  form = this.fb.group({
    type: ['Pickup' as 'Pickup' | 'Delivery', Validators.required],
    customerName: ['', [Validators.required, Validators.minLength(2)]],
    customerPhone: ['', [Validators.required, Validators.minLength(7)]],
    customerAddress: [''],
  });

  ngOnInit(): void {
    // address required only for delivery
    this.form.controls.type.valueChanges.subscribe((type) => {
      const addr = this.form.controls.customerAddress;
      if (type === 'Delivery') addr.setValidators([Validators.required, Validators.minLength(5)]);
      else addr.clearValidators();
      addr.updateValueAndValidity();
    });
  }

  ngAfterViewInit(): void {
    if (this.cart.lines().length === 0) return;
    this.loadSdk()
      .then(() => this.renderButtons())
      .catch(() => this.error.set('Could not load PayPal. Check your connection.'));
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

  private buildRequest(): PaymentOrderRequest {
    const v = this.form.getRawValue();
    return {
      type: v.type!,
      customerName: v.customerName!,
      customerPhone: v.customerPhone!,
      customerAddress: v.customerAddress || undefined,
      items: this.cart.lines().map((l) => ({ menuItemId: l.menuItemId, quantity: l.quantity })),
    };
  }

  private renderButtons(): void {
    if (this.buttonsRendered) return;
    if (!document.getElementById('paypal-button-container')) return;
    this.buttonsRendered = true;

    paypal.Buttons({
      // validate the form before opening the PayPal popup
      onClick: (_data: any, actions: any) => {
        if (this.form.invalid) {
          this.form.markAllAsTouched();
          return actions.reject();
        }
        return actions.resolve();
      },
      // ask our backend to create the PayPal order for the server-computed amount
      createOrder: () => {
        this.error.set(null);
        return firstValueFrom(this.payment.createOrder(this.buildRequest()))
          .then((res) => res.paypalOrderId);
      },
      // after the customer approves, capture and place the order
      onApprove: (data: any) => {
        return firstValueFrom(this.payment.capture(data.orderID, this.buildRequest()))
          .then((order) => {
            this.cart.clear();
            this.router.navigate(['/track', order.trackingCode]);
          })
          .catch(() => this.error.set('We could not finalize your order. Please contact us.'));
      },
      onCancel: () => this.message.set('Payment cancelled. Your cart is saved.'),
      onError: () => this.error.set('Payment failed. Please try again.'),
    }).render('#paypal-button-container');
  }
}