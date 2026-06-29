import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AdminService } from '../../core/admin';
import { Dashboard } from '../../core/models';

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, DecimalPipe, NgxChartsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class DashboardPage implements OnInit {
  private admin = inject(AdminService);

  data = signal<Dashboard | null>(null);
  loading = signal(true);

  // ngx-charts wants a specific shape: { name, series: [{ name, value }] }
  trendSeries = computed(() => {
    const d = this.data();
    if (!d) return [];
    return [
      {
        name: 'Revenue',
        series: d.revenueTrend.map((r) => ({
          name: this.shortDate(r.date),
          value: r.revenue,
        })),
      },
    ];
  });

  // pie/bar charts want a flat { name, value } list
  statusSeries = computed(() => {
    const d = this.data();
    if (!d) return [];
    return d.statusBreakdown.map((s) => ({ name: s.status, value: s.count }));
  });

  ngOnInit(): void {
    this.admin.getDashboard().subscribe({
      next: (d) => {
        this.data.set(d);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  private shortDate(iso: string): string {
    const d = new Date(iso);
    return `${d.getMonth() + 1}/${d.getDate()}`;
  }
}