# Angular Frontend Developer Guide

This guide will walk you through how the Store Management System frontend is structured and explain key Angular concepts to help you build and maintain the application yourself.

## Prerequisites
- **Node.js** (v24.15.0 or later is recommended for Angular 19)
- **Angular CLI**: Install globally via `npm install -g @angular/cli`
- **TypeScript**: Basic knowledge of TypeScript (types, interfaces, classes)
- **RxJS**: Basic understanding of Observables and subscriptions

## Project Structure Overview
The application is located in `StoreManagementSystem.Client` and follows a feature-based architecture.

```text
src/
└── app/
    ├── core/          # Singleton services, models, guards, interceptors (loaded once)
    ├── features/      # Feature-specific components (lazy loaded)
    │   ├── auth/      # Login, Register, Password Reset
    │   ├── dashboard/ # Main landing page with role-based actions
    │   ├── products/  # Product listing and forms
    │   ├── warehouse/ # Warehouse management (Restock, Batches, History)
    │   ├── shelf/     # Shelf management (Move to shelf, Batches)
    │   ├── sales/     # Point of Sale (Process sale, Sales history)
    │   └── rejections/# Rejection records
    └── shared/        # Reusable UI components (Navbar, 404 page)
```

## Key Angular 19 Concepts Used

### 1. Standalone Components
In Angular 19, we don't use `NgModule`. Every component is "standalone", meaning it manages its own dependencies.
```typescript
@Component({
  selector: 'app-example',
  standalone: true,
  imports: [CommonModule, MatButtonModule], // Import needed modules directly here!
  templateUrl: './example.component.html'
})
export class ExampleComponent {}
```

### 2. Dependency Injection via `inject()`
Instead of injecting services in the constructor, modern Angular uses the `inject()` function. This is cleaner and makes inheritance easier.
```typescript
import { inject } from '@angular/core';

export class ExampleComponent {
  // Modern way
  private authService = inject(AuthService);
  private router = inject(Router);
}
```

### 3. Functional Interceptors & Guards
Interceptors (for adding JWT tokens) and Route Guards (for protecting routes) are now simple functions rather than classes.
- **Interceptor:** `src/app/core/interceptors/auth.interceptor.ts` intercepts all HTTP calls to attach the `Authorization: Bearer <token>` header.
- **Guard:** `src/app/core/guards/auth.guard.ts` checks if a user is logged in before allowing them to visit protected routes.

### 4. Lazy Loading Routes
Look at `src/app/app.routes.ts`. We use lazy loading for almost everything. This means the browser only downloads the JavaScript for a feature when the user navigates to it, making the app much faster.
```typescript
export const routes: Routes = [
  { 
    path: 'dashboard', 
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard] 
  }
];
```

## Step-by-Step: Creating a New Feature

If you want to add a new page (e.g., "Supplier Management"), follow these steps:

### Step 1: Generate the Component
Use the Angular CLI to generate a component inside the `features` folder.
```bash
ng generate component features/suppliers/supplier-list
```

### Step 2: Create a Service
Generate a service in the `core/services` folder to handle HTTP calls.
```bash
ng generate service core/services/supplier
```
Inside the service, use `HttpClient` to communicate with the .NET Web API.

### Step 3: Write the Component Logic (.ts)
Inject your service and fetch data when the component initializes.
```typescript
export class SupplierListComponent implements OnInit {
  private supplierService = inject(SupplierService);
  suppliers: Supplier[] = [];

  ngOnInit() {
    this.supplierService.getAll().subscribe(data => {
      this.suppliers = data;
    });
  }
}
```

### Step 4: Write the Template (.html)
Use Angular Material components (like `MatTable`) to display the data. Remember to import the Material modules in your component's `@Component({ imports: [...] })` array!

### Step 5: Add the Route
Open `src/app/app.routes.ts` and add a route for your new component.
```typescript
{ 
  path: 'suppliers', 
  loadComponent: () => import('./features/suppliers/supplier-list/supplier-list.component').then(m => m.SupplierListComponent),
  canActivate: [authGuard] 
}
```

### Step 6: Add it to the Navbar or Dashboard
Open `src/app/shared/navbar/navbar.component.html` and add a button linking to `/suppliers`. Use the `hasRole()` method if you want to restrict access to certain roles.

## Best Practices
1. **Always unsubscribe:** If you manually subscribe to an Observable (other than HTTP calls which auto-complete), make sure to unsubscribe in `ngOnDestroy` to prevent memory leaks, or use the `takeUntilDestroyed()` operator.
2. **Use Reactive Forms:** For any form, use `FormGroup` and `FormControl` from `@angular/forms` (like we did in the Login and Register components). They are much more powerful and testable than template-driven forms.
3. **Keep Components Clean:** Move business logic to Services. Components should mostly handle UI state and calling Service methods.
