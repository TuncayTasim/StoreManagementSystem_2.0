import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';

/**
 * Application-level configuration.
 * 
 * This is where we register global providers:
 * - provideRouter: sets up the Angular router with our route definitions
 * - provideHttpClient: enables HttpClient for API calls, with our JWT interceptor
 * - provideAnimationsAsync: enables Angular Material animations
 * - provideZoneChangeDetection: configures Angular's change detection (with event coalescing for performance)
 */
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync(),
  ]
};
