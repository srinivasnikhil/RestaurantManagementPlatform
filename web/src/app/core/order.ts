import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from './models';
import { API_BASE } from './api.config';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private base = `${API_BASE}/orders`;

  placeOrder(type: string): Observable<Order> {
    return this.http.post<Order>(this.base, { type });
  }

  getMyOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.base);
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.base}/${id}`);
  }
}