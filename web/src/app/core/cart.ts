import { Injectable, computed, signal, effect } from '@angular/core';
import { MenuItem } from './models';

export interface LocalCartLine {
  menuItemId: number;
  name: string;
  unitPrice: number;   // display only; the server reprices at checkout
  quantity: number;
}

const STORAGE_KEY = 'dosthi_cart';

@Injectable({ providedIn: 'root' })
export class CartService {
  lines = signal<LocalCartLine[]>(this.loadFromStorage());

  itemCount = computed(() => this.lines().reduce((sum, l) => sum + l.quantity, 0));
  subtotal = computed(() => this.lines().reduce((sum, l) => sum + l.unitPrice * l.quantity, 0));

  constructor() {
    // whenever the cart changes, save it to the browser
    effect(() => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(this.lines()));
    });
  }

  add(item: MenuItem, quantity = 1): void {
    this.lines.update((lines) => {
      const existing = lines.find((l) => l.menuItemId === item.id);
      if (existing) {
        return lines.map((l) =>
          l.menuItemId === item.id ? { ...l, quantity: l.quantity + quantity } : l
        );
      }
      return [...lines, { menuItemId: item.id, name: item.name, unitPrice: item.price, quantity }];
    });
  }

  setQuantity(menuItemId: number, quantity: number): void {
    if (quantity < 1) {
      this.remove(menuItemId);
      return;
    }
    this.lines.update((lines) =>
      lines.map((l) => (l.menuItemId === menuItemId ? { ...l, quantity } : l))
    );
  }

  quantityOf(menuItemId: number): number {
    return this.lines().find((l) => l.menuItemId === menuItemId)?.quantity ?? 0;
  }

  remove(menuItemId: number): void {
    this.lines.update((lines) => lines.filter((l) => l.menuItemId !== menuItemId));
  }

  clear(): void {
    this.lines.set([]);
  }

  private loadFromStorage(): LocalCartLine[] {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      return raw ? JSON.parse(raw) : [];
    } catch {
      return [];
    }
  }
}