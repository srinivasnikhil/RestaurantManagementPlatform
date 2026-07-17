import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CartService } from '../core/cart';
import { OrderService } from '../core/order';

@Component({
  selector: 'app-checkout',
  imports: [CurrencyPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit {
  protected cart = inject(CartService);
  private orders = inject(OrderService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  placing = signal(false);
  error = signal<string | null>(null);

  form = this.fb.group({
    type: ['Pickup' as 'Pickup' | 'Delivery', Validators.required],
    customerName: ['', [Validators.required, Validators.minLength(2)]],
    customerPhone: ['', [Validators.required, Validators.minLength(7)]],
    customerAddress: [''],
  });

  ngOnInit(): void {
    // address is only required for delivery
    this.form.controls.type.valueChanges.subscribe((type) => {
      const addr = this.form.controls.customerAddress;
      if (type === 'Delivery') addr.setValidators([Validators.required, Validators.minLength(5)]);
      else addr.clearValidators();
      addr.updateValueAndValidity();
    });
  }

  placeOrder(): void {
    if (this.form.invalid || this.cart.lines().length === 0) {
      this.form.markAllAsTouched();
      return;
    }
    this.placing.set(true);
    this.error.set(null);

    const v = this.form.getRawValue();
    this.orders.placeGuestOrder({
      type: v.type!,
      customerName: v.customerName!,
      customerPhone: v.customerPhone!,
      customerAddress: v.customerAddress || undefined,
      items: this.cart.lines().map((l) => ({ menuItemId: l.menuItemId, quantity: l.quantity })),
    }).subscribe({
      next: (order) => {
        this.cart.clear();
        this.router.navigate(['/track', order.trackingCode]);
      },
      error: (err) => {
        this.error.set(err.error ?? 'Could not place your order. Please try again.');
        this.placing.set(false);
      },
    });
  }
}