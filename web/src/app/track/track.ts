import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../core/order';
import { Order } from '../core/models';

@Component({
  selector: 'app-track',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './track.html',
  styleUrl: './track.css',
})
export class Track implements OnInit {
  protected orders = inject(OrderService);
  private route = inject(ActivatedRoute);

  order = signal<Order | null>(null);
  loading = signal(true);
  notFound = signal(false);

  private readonly flow = ['Placed', 'Confirmed', 'Preparing', 'Ready', 'Completed'];

  steps = computed(() => {
    const current = this.order()?.status;
    const idx = current ? this.flow.indexOf(current) : -1;
    return this.flow.map((name, i) => ({ name, done: idx >= 0 && i <= idx }));
  });

  isCancelled = computed(() => this.order()?.status === 'Cancelled');

  ngOnInit(): void {
    const code = this.route.snapshot.paramMap.get('code')!;
    this.orders.trackByCode(code).subscribe({
      next: (o) => { this.order.set(o); this.loading.set(false); },
      error: () => { this.notFound.set(true); this.loading.set(false); },
    });
  }

  openReceipt(): void {
    const o = this.order();
    if (o) window.open(this.orders.receiptUrlByCode(o.trackingCode), '_blank');
  }
}