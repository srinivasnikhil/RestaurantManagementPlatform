import { Component, signal, inject } from '@angular/core';
import { RouterLink, RouterOutlet, Router } from '@angular/router';
import { Auth } from './core/auth';
import { CartService } from './core/cart';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected auth = inject(Auth);
  protected cart = inject(CartService);
  private router = inject(Router);

  constructor() {
    // if the user is already logged in from a previous session, load their cart
    if (this.auth.isLoggedIn()) {
      this.cart.load();
    }
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/']);
  }
}
