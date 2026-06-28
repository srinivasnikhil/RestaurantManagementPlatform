import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { OrderService } from '../core/order';
import { Order } from '../core/models';

@Component({
  selector: 'app-order-detail',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css',
})
export class OrderDetail implements OnInit {
  private orderService = inject(OrderService);
  private route = inject(ActivatedRoute);

  order = signal<Order | null>(null);
  loading = signal(true);

  // the happy-path status flow, for the stepper
  private readonly flow = ['Placed', 'Confirmed', 'Preparing', 'Ready', 'Completed'];

  steps = computed(() => {
    const current = this.order()?.status;
    const currentIndex = current ? this.flow.indexOf(current) : -1;
    return this.flow.map((name, i) => ({
      name,
      done: currentIndex >= 0 && i <= currentIndex,
    }));
  });

  isCancelled = computed(() => this.order()?.status === 'Cancelled');

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.orderService.getById(id).subscribe({
      next: (o) => {
        this.order.set(o);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}