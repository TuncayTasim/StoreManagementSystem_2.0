import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

/**
 * Application route definitions.
 * 
 * Routes use lazy loading via `loadComponent` — the component is only downloaded
 * when the user navigates to that route. This improves initial load performance.
 * 
 * Protected routes use `canActivate: [authGuard]` to redirect unauthenticated users.
 */
export const routes: Routes = [
  // ─── Public routes (no guard) ─────────────────────
  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent) },
  { path: 'confirm-email', loadComponent: () => import('./features/auth/confirm-email/confirm-email.component').then(m => m.ConfirmEmailComponent) },
  { path: 'forgot-password', loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) },
  { path: 'reset-password', loadComponent: () => import('./features/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) },

  // ─── Protected routes (require login) ─────────────
  { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent), canActivate: [authGuard] },
  { path: 'products', loadComponent: () => import('./features/products/product-list/product-list.component').then(m => m.ProductListComponent), canActivate: [authGuard] },
  { path: 'change-password', loadComponent: () => import('./features/auth/change-password/change-password.component').then(m => m.ChangePasswordComponent), canActivate: [authGuard] },

  // Warehouse
  { path: 'warehouse/restock', loadComponent: () => import('./features/warehouse/warehouse-restock/warehouse-restock.component').then(m => m.WarehouseRestockComponent), canActivate: [authGuard] },
  { path: 'warehouse/batches', loadComponent: () => import('./features/warehouse/warehouse-batches/warehouse-batches.component').then(m => m.WarehouseBatchesComponent), canActivate: [authGuard] },
  { path: 'warehouse/history', loadComponent: () => import('./features/warehouse/warehouse-history/warehouse-history.component').then(m => m.WarehouseHistoryComponent), canActivate: [authGuard] },

  // Shelf
  { path: 'shelf/move', loadComponent: () => import('./features/shelf/move-to-shelf/move-to-shelf.component').then(m => m.MoveToShelfComponent), canActivate: [authGuard] },
  { path: 'shelf/batches', loadComponent: () => import('./features/shelf/shelf-batches/shelf-batches.component').then(m => m.ShelfBatchesComponent), canActivate: [authGuard] },
  { path: 'shelf/history', loadComponent: () => import('./features/shelf/shelf-history/shelf-history.component').then(m => m.ShelfHistoryComponent), canActivate: [authGuard] },

  // Sales
  { path: 'sales/sell', loadComponent: () => import('./features/sales/sell/sell.component').then(m => m.SellComponent), canActivate: [authGuard] },
  { path: 'sales/history', loadComponent: () => import('./features/sales/sales-history/sales-history.component').then(m => m.SalesHistoryComponent), canActivate: [authGuard] },

  // Rejections
  { path: 'rejections', loadComponent: () => import('./features/rejections/rejection-list/rejection-list.component').then(m => m.RejectionListComponent), canActivate: [authGuard] },

  // ─── Default & fallback routes ────────────────────
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', loadComponent: () => import('./shared/not-found/not-found.component').then(m => m.NotFoundComponent) },
];
