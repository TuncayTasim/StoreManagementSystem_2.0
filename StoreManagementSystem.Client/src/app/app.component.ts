import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component';

/**
 * Root component of the application.
 * 
 * It contains:
 * - The navbar (shows only when logged in)
 * - <router-outlet> which renders the current route's component
 */
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `
    <app-navbar />
    <router-outlet />
  `,
  styles: []
})
export class AppComponent {
  title = 'StoreManagementSystem';
}
