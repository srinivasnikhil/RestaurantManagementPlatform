import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from './auth';

export const staffGuard: CanActivateFn = () => {
  const auth = inject(Auth);
  const router = inject(Router);

  const role = auth.currentUser()?.role;
  if (role === 'Admin' || role === 'Employee') return true;

  router.navigate(['/login']);
  return false;
};