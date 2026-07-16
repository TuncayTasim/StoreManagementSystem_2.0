import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../core/services/auth.service';

/**
 * Navigation bar component shown at the top of the page.
 * 
 * It uses AuthService.currentUser$ (an Observable) to reactively show/hide
 * links based on whether the user is logged in and what their role is.
 * 
 * Role IDs: Admin=1, Shelf Manager=2, Warehouse Manager=3, Sales Manager=4
 */
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  auth = inject(AuthService);
  private router = inject(Router);

  /** Check if current user's role is in the allowed list */
  hasRole(...roles: number[]): boolean {
    const roleId = this.auth.getRoleId();
    return roleId !== null && roles.includes(roleId);
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
