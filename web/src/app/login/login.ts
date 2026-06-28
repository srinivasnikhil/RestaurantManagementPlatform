import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../core/auth';
import { CartService } from '../core/cart';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private fb = inject(FormBuilder);
  private auth = inject(Auth);
  private cart = inject(CartService);
  private router = inject(Router);

  error = signal<string | null>(null);
  submitting = signal(false);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.error.set(null);

    const { email, password } = this.form.getRawValue();
    this.auth.login(email!, password!).subscribe({
      next: () => {
        this.cart.load(); 
        this.router.navigate(['/']) 
      },
      error: () => {
        this.error.set('Invalid email or password.');
        this.submitting.set(false);
      },
    });
  }
}