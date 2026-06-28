import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartService } from '../core/cart';

@Component({
  selector: 'app-cart-page',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './cart-page.html',
  styleUrl: './cart-page.css',
})
export class CartPage implements OnInit {
  protected cart = inject(CartService);

  ngOnInit(): void {
    this.cart.load();
  }

  changeQty(cartItemId: number, quantity: number): void {
    if (quantity < 1) return;
    this.cart.updateQuantity(cartItemId, quantity);
  }
}