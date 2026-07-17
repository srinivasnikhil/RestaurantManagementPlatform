import { Routes } from '@angular/router';
import { Menu } from './menu/menu';
import { Login } from './login/login';
import { CartPage } from './cart-page/cart-page';
import { Checkout } from './checkout/checkout';
import { Track } from './track/track';
import { AdminLayout } from './admin/admin-layout/admin-layout';
import { DashboardPage } from './admin/dashboard/dashboard';
import { AdminOrders } from './admin/admin-orders/admin-orders';
import { AdminMenu } from './admin/admin-menu/admin-menu';
import { Kitchen } from './admin/kitchen/kitchen';
import { adminGuard } from './core/admin-guard';
import { staffGuard } from './core/staff-guard';
import { Pos } from './admin/pos/pos';
import { Receipt } from './receipt/receipt';

export const routes: Routes = [
  { path: '', component: Menu },
  { path: 'cart', component: CartPage },
  { path: 'checkout', component: Checkout },
  { path: 'track/:code', component: Track },
  { path: 'receipt/:code', component: Receipt },
  { path: 'login', component: Login },        // staff login
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [staffGuard],
    children: [
        { path: '', redirectTo: 'orders', pathMatch: 'full' },
        { path: 'pos', component: Pos },
        { path: 'orders', component: AdminOrders },
        { path: 'kitchen', component: Kitchen },
        { path: 'menu', component: AdminMenu, canActivate: [adminGuard] },       // admin-only
        { path: 'dashboard', component: DashboardPage, canActivate: [adminGuard] }, // admin-only
    ],
  },
];