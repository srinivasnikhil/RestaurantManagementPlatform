import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../core/auth';
import { CartService } from '../core/cart';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private fb = inject(FormBuilder);
  private auth = inject(Auth);
  private cart = inject(CartService);
  private router = inject(Router);

  error = signal<string | null>(null);
  submitting = signal(false);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.error.set(null);

    const { name, email, password } = this.form.getRawValue();
    this.auth.register(name!, email!, password!).subscribe({
      next: () => {
        this.cart.load();
        this.router.navigate(['/'])
      },
      error: (err) => {
        this.error.set(err.status === 409 ? 'That email is already registered.' : 'Something went wrong.');
        this.submitting.set(false);
      },
    });
  }
}