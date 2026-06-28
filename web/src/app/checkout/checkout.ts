import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { CartService } from '../core/cart';
import { OrderService } from '../core/order';

@Component({
  selector: 'app-checkout',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class Checkout implements OnInit {
  protected cart = inject(CartService);
  private orders = inject(OrderService);
  private router = inject(Router);

  orderType = signal<'Pickup' | 'Delivery'>('Pickup');
  placing = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.cart.load();
  }

  setType(type: 'Pickup' | 'Delivery'): void {
    this.orderType.set(type);
  }

  placeOrder(): void {
    this.placing.set(true);
    this.error.set(null);

    this.orders.placeOrder(this.orderType()).subscribe({
      next: (order) => {
        this.cart.cart.set(null);          // backend cleared it; clear our copy too
        this.router.navigate(['/orders', order.id]);  // go to the tracking view
      },
      error: () => {
        this.error.set('Could not place your order. Please try again.');
        this.placing.set(false);
      },
    });
  }
}