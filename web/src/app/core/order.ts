import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from './models';
import { API_BASE } from './api.config';

export interface PlaceOrderRequest {
  type: 'Pickup' | 'Delivery' | 'DineIn' | 'Takeaway';
  customerName: string;
  customerPhone: string;
  customerAddress?: string;
  items: { menuItemId: number; quantity: number }[];
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private base = `${API_BASE}/orders`;

  placeGuestOrder(request: PlaceOrderRequest): Observable<Order> {
    return this.http.post<Order>(this.base, request);
  }

  trackByCode(code: string): Observable<Order> {
    return this.http.get<Order>(`${this.base}/track/${code}`);
  }

  placeStaffOrder(request: PlaceOrderRequest): Observable<Order> {
    return this.http.post<Order>(`${this.base}/staff`, request);
  }

  // public receipt: just a URL, no auth needed (browser downloads it)
  receiptUrlByCode(code: string): string {
    return `${API_BASE}/orders/track/${code}/receipt`;
  }

  // staff receipt by id: needs the token, so fetch as a blob
  downloadReceiptById(id: number): Observable<Blob> {
    return this.http.get(`${this.base}/${id}/receipt`, { responseType: 'blob' });
  }
}