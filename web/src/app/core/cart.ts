import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Cart } from './models';
import { API_BASE } from './api.config';

@Injectable({ providedIn: 'root' })
export class CartService {
  private http = inject(HttpClient);
  private base = `${API_BASE}/cart`;

  cart = signal<Cart | null>(null);

  // total number of items, for the header badge
  itemCount = computed(() =>
    this.cart()?.items.reduce((sum, i) => sum + i.quantity, 0) ?? 0
  );

  load(): void {
    this.http.get<Cart>(this.base).subscribe({
      next: (c) => this.cart.set(c),
      error: () => this.cart.set(null),
    });
  }

  addItem(menuItemId: number, quantity = 1): void {
    this.http.post<Cart>(`${this.base}/items`, { menuItemId, quantity })
      .subscribe({ next: (c) => this.cart.set(c) });
  }

  updateQuantity(cartItemId: number, quantity: number): void {
    this.http.put<Cart>(`${this.base}/items/${cartItemId}`, { quantity })
      .subscribe({ next: (c) => this.cart.set(c) });
  }

  removeItem(cartItemId: number): void {
    this.http.delete(`${this.base}/items/${cartItemId}`)
      .subscribe({ next: () => this.load() });
  }

  clear(): void {
    this.http.delete(`${this.base}/clear`)
      .subscribe({ next: () => this.cart.set(null) });
  }
}