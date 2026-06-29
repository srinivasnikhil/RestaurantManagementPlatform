import { Routes } from '@angular/router';
import { Menu } from './menu/menu';
import { Login } from './login/login';
import { Register } from './register/register';
import { CartPage } from './cart-page/cart-page';
import { authGuard } from './core/auth-guard';
import { Checkout } from './checkout/checkout';
import { Orders } from './orders/orders';
import { OrderDetail } from './order-detail/order-detail';
import { AdminLayout } from './admin/admin-layout/admin-layout';
import { adminGuard } from './core/admin-guard';
import { AdminOrders } from './admin/admin-orders/admin-orders';
import { Kitchen } from './admin/kitchen/kitchen';
import { AdminMenu } from './admin/admin-menu/admin-menu';
import { DashboardPage } from './admin/dashboard/dashboard';

export const routes: Routes = [
    { path: '', component: Menu},
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'cart', component: CartPage, canActivate: [authGuard]},
    { path: 'checkout', component: Checkout, canActivate: [authGuard]},
    { path: 'orders', component: Orders, canActivate: [authGuard]},
    { path: 'orders/:id', component: OrderDetail, canActivate: [authGuard]},
    {
        path: 'admin',
        component: AdminLayout,
        canActivate: [adminGuard],
        children: [
            { path: '', redirectTo: 'orders', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardPage },
            { path: 'orders', component: AdminOrders },
            { path: 'kitchen', component: Kitchen },
            { path: 'menu', component: AdminMenu },
        ]
    }
];
