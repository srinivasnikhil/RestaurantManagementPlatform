import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { MenuItem, Order, Dashboard } from './models';
import { API_BASE } from './api.config';


export interface MenuItemPayload {
  categoryId: number;
  name: string;
  description: string;
  price: number;
  imageUrl?: string | null;
  isVeg: boolean;
  spiceLevel: number;
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  private http = inject(HttpClient);

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${API_BASE}/orders/all`);
  }

  updateOrderStatus(id: number, status: string): Observable<void> {
    return this.http.patch<void>(`${API_BASE}/orders/${id}/status`, { status });
  }

  // --- menu management ---
  createMenuItem(payload: MenuItemPayload): Observable<MenuItem> {
    return this.http.post<MenuItem>(`${API_BASE}/menu-items`, payload);
  }
  updateMenuItem(id: number, payload: MenuItemPayload & { isAvailable: boolean }): Observable<void> {
    return this.http.put<void>(`${API_BASE}/menu-items/${id}`, payload);
  }
  deleteMenuItem(id: number): Observable<void> {
    return this.http.delete<void>(`${API_BASE}/menu-items/${id}`);
  }
  setAvailability(id: number, isAvailable: boolean): Observable<void> {
    return this.http.patch<void>(`${API_BASE}/menu-items/${id}/availability`, { isAvailable });
  }

  getDashboard(): Observable<Dashboard> {
    return this.http.get<Dashboard>(`${API_BASE}/admin/dashboard`);
 }
}