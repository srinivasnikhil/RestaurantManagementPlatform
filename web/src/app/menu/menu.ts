import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { MenuApi } from '../core/menu-api';
import { Category, MenuItem } from '../core/models';
import { Router } from '@angular/router';
import { Auth } from '../core/auth';
import { CartService } from '../core/cart';

@Component({
  selector: 'app-menu',
  imports: [CurrencyPipe],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})
export class Menu implements OnInit {
  private menuApi = inject(MenuApi);
  private cart = inject(CartService);
  private auth = inject(Auth);
  private router = inject(Router);

  items = signal<MenuItem[]>([]);
  categories = signal<Category[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);

  // current filter state
  selectedCategoryId = signal<number | null>(null);
  vegOnly = signal(false);
  search = signal('');

  ngOnInit(): void {
    this.loadCategories();
    this.loadItems();
  }

  private loadCategories(): void {
    this.menuApi.getCategories().subscribe({
      next: (data) => this.categories.set(data),
      error: () => {}, // tabs are optional, don't block the menu
    });
  }

  private loadItems(): void {
    this.loading.set(true);
    const categoryId = this.selectedCategoryId() ?? undefined;
    const isVeg = this.vegOnly() ? true : undefined;
    const search = this.search().trim() || undefined;

    this.menuApi.getMenuItems(categoryId, isVeg, search).subscribe({
      next: (data) => {
        this.items.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load the menu. Is the API running?');
        this.loading.set(false);
      },
    });
  }

  // called by the UI when a filter changes
  selectCategory(id: number | null): void {
    this.selectedCategoryId.set(id);
    this.loadItems();
  }

  toggleVeg(): void {
    this.vegOnly.update((v) => !v);
    this.loadItems();
  }

  onSearch(value: string): void {
    this.search.set(value);
    this.loadItems();
  }

  addToCart(item: MenuItem): void {
    // must be logged in to have a cart
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.cart.addItem(item.id, 1);
  }
}