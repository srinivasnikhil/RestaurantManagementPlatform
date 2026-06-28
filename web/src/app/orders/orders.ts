import { Component, inject, signal, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderService } from '../core/order';
import { Order } from '../core/models';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class Orders implements OnInit {
  private orderService = inject(OrderService);

  orders = signal<Order[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.orderService.getMyOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}