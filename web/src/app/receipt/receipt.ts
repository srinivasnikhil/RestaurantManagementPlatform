import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { OrderService } from '../core/order';
import { ActivatedRoute } from '@angular/router';
import { Order } from '../core/models';

@Component({
  selector: 'app-receipt',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './receipt.html',
  styleUrl: './receipt.css',
})
export class Receipt implements OnInit {
    private orders = inject(OrderService);
    private route = inject(ActivatedRoute);

    order = signal<Order | null>(null);
    notFound = signal(false);

    ngOnInit(): void {
        const code = this.route.snapshot.paramMap.get('code')!;
        this.orders.trackByCode(code).subscribe({
          next: (o) => this.order.set(o),
          error: () => this.notFound.set(true),
        });
    }

    print(): void {
      window.print();
    }
}
