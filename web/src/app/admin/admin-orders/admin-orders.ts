import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { AdminService } from '../../core/admin';
import { Order } from '../../core/models';

@Component({
  selector: 'app-admin-orders',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.css',
})
export class AdminOrders implements OnInit {
  private admin = inject(AdminService);

  orders = signal<Order[]>([]);
  loading = signal(true);
  filter = signal('All');

  readonly statuses = ['Placed', 'Confirmed', 'Preparing', 'Ready', 'Completed', 'Cancelled'];

  visibleOrders = computed(() => {
    const f = this.filter();
    return f === 'All' ? this.orders() : this.orders().filter((o) => o.status === f);
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.admin.getAllOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  setFilter(f: string): void {
    this.filter.set(f);
  }

  changeStatus(order: Order, status: string): void {
    if (status === order.status) return;
    this.admin.updateOrderStatus(order.id, status).subscribe({
      next: () => {
        // reflect the change in the list immediately
        this.orders.update((list) =>
          list.map((o) => (o.id === order.id ? { ...o, status } : o))
        );
      },
    });
  }
}