import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

/**
 * Functional HTTP interceptor that attaches the JWT token
 * to the Authorization header of every outgoing request.
 *
 * Angular 19 uses functional interceptors instead of class-based ones.
 * This function is registered in app.config.ts via provideHttpClient(withInterceptors([...])).
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (token) {
    // Clone the request and add the Authorization header.
    // HTTP requests are immutable in Angular, so we must clone to modify.
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq);
  }

  // No token — pass the request through unchanged
  return next(req);
};
