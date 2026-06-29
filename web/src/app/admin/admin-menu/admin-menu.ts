import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MenuApi } from '../../core/menu-api';
import { AdminService, MenuItemPayload } from '../../core/admin';
import { Category, MenuItem } from '../../core/models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-admin-menu',
  imports: [CurrencyPipe, ReactiveFormsModule],
  templateUrl: './admin-menu.html',
  styleUrl: './admin-menu.css',
})
export class AdminMenu implements OnInit {
  private menuApi = inject(MenuApi);
  private admin = inject(AdminService);
  private fb = inject(FormBuilder);

  items = signal<MenuItem[]>([]);
  categories = signal<Category[]>([]);
  loading = signal(true);

  // modal state
  showForm = signal(false);
  editingId = signal<number | null>(null);
  saving = signal(false);

  isEditing = computed(() => this.editingId() !== null);

  form = this.fb.group({
    categoryId: [null as number | null, [Validators.required]],
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: ['', [Validators.maxLength(500)]],
    price: [0, [Validators.required, Validators.min(0.01)]],
    spiceLevel: [0, [Validators.min(0), Validators.max(3)]],
    isVeg: [false],
    imageUrl: [''],
    isAvailable: [true],
  });

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading.set(true);
    this.menuApi.getCategories().subscribe({ next: (c) => this.categories.set(c) });
    this.menuApi.getMenuItems().subscribe({
      next: (data) => {
        this.items.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  openCreate(): void {
    this.editingId.set(null);
    this.form.reset({
      categoryId: null, name: '', description: '', price: 0,
      spiceLevel: 0, isVeg: false, imageUrl: '', isAvailable: true,
    });
    this.showForm.set(true);
  }

  openEdit(item: MenuItem): void {
    this.editingId.set(item.id);
    this.form.reset({
      categoryId: item.categoryId,
      name: item.name,
      description: item.description,
      price: item.price,
      spiceLevel: item.spiceLevel,
      isVeg: item.isVeg,
      imageUrl: item.imageUrl ?? '',
      isAvailable: item.isAvailable,
    });
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();

    const payload: MenuItemPayload = {
      categoryId: v.categoryId!,
      name: v.name!,
      description: v.description ?? '',
      price: v.price!,
      spiceLevel: v.spiceLevel ?? 0,
      isVeg: v.isVeg ?? false,
      imageUrl: v.imageUrl || null,
    };

    const id = this.editingId();
    const request: Observable<unknown> = id
      ? this.admin.updateMenuItem(id, { ...payload, isAvailable: v.isAvailable ?? true })
      : this.admin.createMenuItem(payload);

    request.subscribe({
      next: () => {
        this.saving.set(false);
        this.showForm.set(false);
        this.loadAll();
      },
      error: () => this.saving.set(false),
    });
  }

  toggleAvailability(item: MenuItem): void {
    const next = !item.isAvailable;
    this.admin.setAvailability(item.id, next).subscribe({
      next: () => {
        this.items.update((list) =>
          list.map((i) => (i.id === item.id ? { ...i, isAvailable: next } : i))
        );
      },
    });
  }

  remove(item: MenuItem): void {
    if (!confirm(`Delete "${item.name}"? This cannot be undone.`)) return;
    this.admin.deleteMenuItem(item.id).subscribe({
      next: () => this.items.update((list) => list.filter((i) => i.id !== item.id)),
    });
  }
}