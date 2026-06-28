import { Routes } from '@angular/router';
import { Menu } from './menu/menu';
import { Login } from './login/login';
import { Register } from './register/register';
import { CartPage } from './cart-page/cart-page';
import { authGuard } from './core/auth-guard';
import { Checkout } from './checkout/checkout';
import { Orders } from './orders/orders';
import { OrderDetail } from './order-detail/order-detail';

export const routes: Routes = [
    { path: '', component: Menu},
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'cart', component: CartPage, canActivate: [authGuard]},
    { path: 'checkout', component: Checkout, canActivate: [authGuard]},
    { path: 'orders', component: Orders, canActivate: [authGuard]},
    { path: 'orders/:id', component: OrderDetail, canActivate: [authGuard]}
];
