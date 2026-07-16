import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Functional route guard that prevents unauthenticated users
 * from accessing protected routes.
 *
 * If the user is not logged in, they are redirected to /login.
 * This guard is used in the route definitions (app.routes.ts).
 */
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  // Redirect to login page if not authenticated
  router.navigate(['/login']);
  return false;
};
