import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Category, MenuItem } from './models';
import { API_BASE } from './api.config';

@Injectable({ providedIn: 'root' })
export class MenuApi {
  private http = inject(HttpClient);

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${API_BASE}/categories`);
  }

  getMenuItems(categoryId?: number, isVeg?: boolean, search?: string): Observable<MenuItem[]> {
    const params: Record<string, string | number | boolean> = {};
    if (categoryId != null) params['categoryId'] = categoryId;
    if (isVeg != null) params['isVeg'] = isVeg;
    if (search) params['search'] = search;
    return this.http.get<MenuItem[]>(`${API_BASE}/menu-items`, { params });
  }
}