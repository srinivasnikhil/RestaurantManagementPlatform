import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_BASE } from './api.config';

export interface UserInfo {
  id: number;
  name: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  user: UserInfo;
}

@Injectable({ providedIn: 'root' })
export class Auth {
  private http = inject(HttpClient);
  private readonly tokenKey = 'dosthi_token';
  private readonly userKey = 'dosthi_user';

  // start from storage so a page refresh keeps you logged in
  currentUser = signal<UserInfo | null>(this.loadUser());
  isLoggedIn = computed(() => this.currentUser() !== null);
  isAdmin = computed(() => this.currentUser()?.role === 'Admin');

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_BASE}/auth/login`, { email, password })
      .pipe(tap((res) => this.storeSession(res)));
  }

  register(name: string, email: string, password: string): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_BASE}/auth/register`, { name, email, password })
      .pipe(tap((res) => this.storeSession(res)));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private storeSession(res: AuthResponse): void {
    localStorage.setItem(this.tokenKey, res.token);
    localStorage.setItem(this.userKey, JSON.stringify(res.user));
    this.currentUser.set(res.user);
  }

  private loadUser(): UserInfo | null {
    const raw = localStorage.getItem(this.userKey);
    return raw ? JSON.parse(raw) : null;
  }
}