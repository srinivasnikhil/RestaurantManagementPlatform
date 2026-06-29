import { Component, inject, signal, computed, OnInit, OnDestroy } from '@angular/core';
import { DatePipe } from '@angular/common';
import { AdminService } from '../../core/admin';
import { Order } from '../../core/models';

@Component({
  selector: 'app-kitchen',
  imports: [DatePipe],
  templateUrl: './kitchen.html',
  styleUrl: './kitchen.css',
})
export class Kitchen implements OnInit, OnDestroy {
  private admin = inject(AdminService);

  orders = signal<Order[]>([]);
  loading = signal(true);
  lastUpdated = signal<Date | null>(null);

  private timer?: ReturnType<typeof setInterval>;

  // the active stages shown as columns
  readonly columns: { key: string; label: string; next: string | null }[] = [
    { key: 'Placed', label: 'New', next: 'Confirmed' },
    { key: 'Confirmed', label: 'Confirmed', next: 'Preparing' },
    { key: 'Preparing', label: 'Preparing', next: 'Ready' },
    { key: 'Ready', label: 'Ready for pickup', next: 'Completed' },
  ];

  // group the orders by status so each column can read its own list
  grouped = computed(() => {
    const map: Record<string, Order[]> = { Placed: [], Confirmed: [], Preparing: [], Ready: [] };
    for (const o of this.orders()) {
      if (map[o.status]) map[o.status].push(o);
    }
    return map;
  });

  ngOnInit(): void {
    this.load();
    // refresh every 8 seconds
    this.timer = setInterval(() => this.load(true), 8000);
  }

  ngOnDestroy(): void {
    // stop polling when we leave the page
    if (this.timer) clearInterval(this.timer);
  }

  load(silent = false): void {
    if (!silent) this.loading.set(true);
    this.admin.getAllOrders().subscribe({
      next: (data) => {
        // keep only the active stages
        const active = data.filter((o) =>
          ['Placed', 'Confirmed', 'Preparing', 'Ready'].includes(o.status)
        );
        this.orders.set(active);
        this.loading.set(false);
        this.lastUpdated.set(new Date());
      },
      error: () => this.loading.set(false),
    });
  }

  advance(order: Order, nextStatus: string | null): void {
    if (!nextStatus) return;
    this.admin.updateOrderStatus(order.id, nextStatus).subscribe({
      next: () => {
        this.orders.update((list) => {
          // if it moved past "Ready" (to Completed), drop it off the board
          if (nextStatus === 'Completed') {
            return list.filter((o) => o.id !== order.id);
          }
          return list.map((o) => (o.id === order.id ? { ...o, status: nextStatus } : o));
        });
      },
    });
  }

  cancel(order: Order): void {
    this.admin.updateOrderStatus(order.id, 'Cancelled').subscribe({
      next: () => this.orders.update((list) => list.filter((o) => o.id !== order.id)),
    });
  }

  minutesAgo(iso: string): number {
    return Math.max(0, Math.floor((Date.now() - new Date(iso).getTime()) / 60000));
  }
}