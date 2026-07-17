import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from './models';
import { API_BASE } from './api.config';

export interface PaymentOrderRequest {
  type: 'Pickup' | 'Delivery' | 'DineIn' | 'Takeaway';
  customerName: string;
  customerPhone: string;
  customerAddress?: string;
  items: { menuItemId: number; quantity: number }[];
}

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private http = inject(HttpClient);
  private base = `${API_BASE}/payments/paypal`;

  createOrder(request: PaymentOrderRequest): Observable<{ paypalOrderId: string }> {
    return this.http.post<{ paypalOrderId: string }>(`${this.base}/create-order`, request);
  }

  capture(paypalOrderId: string, order: PaymentOrderRequest): Observable<Order> {
    return this.http.post<Order>(`${this.base}/capture`, { paypalOrderId, order });
  }

  createStaffOrder(request: PaymentOrderRequest): Observable<{ paypalOrderId: string }> {
    return this.http.post<{ paypalOrderId: string }>(`${this.base}/staff/create-order`, request);
  }

  captureStaff(paypalOrderId: string, order: PaymentOrderRequest): Observable<Order> {
    return this.http.post<Order>(`${this.base}/staff/capture`, { paypalOrderId, order });
  }
}