import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/services/auth.service';
import { UserResponseDTO } from '../../core/models/user.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, MatCardModule, MatIconModule, MatButtonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private auth = inject(AuthService);

  user: UserResponseDTO | null = null;

  // Define quick-action cards with role restrictions
  // roleIds: Admin=1, Shelf=2, Warehouse=3, Sales=4
  cards = [
    { title: 'Products', icon: 'inventory_2', description: 'View and manage products', route: '/products', roles: [1, 2, 3, 4] },
    { title: 'Warehouse Restock', icon: 'warehouse', description: 'Restock products into warehouse', route: '/warehouse/restock', roles: [1, 3] },
    { title: 'Warehouse Batches', icon: 'view_list', description: 'View warehouse batches', route: '/warehouse/batches', roles: [1, 3] },
    { title: 'Warehouse History', icon: 'history', description: 'Warehouse audit log', route: '/warehouse/history', roles: [1, 3] },
    { title: 'Move to Shelf', icon: 'shelves', description: 'Move stock from warehouse to shelf', route: '/shelf/move', roles: [1, 2] },
    { title: 'Shelf Batches', icon: 'view_list', description: 'View shelf batches', route: '/shelf/batches', roles: [1, 2] },
    { title: 'Shelf History', icon: 'history', description: 'Shelf audit log', route: '/shelf/history', roles: [1, 2] },
    { title: 'Process Sale', icon: 'point_of_sale', description: 'Sell products from the shelf', route: '/sales/sell', roles: [1, 4] },
    { title: 'Sales History', icon: 'receipt_long', description: 'View sales records & totals', route: '/sales/history', roles: [1, 4] },
    { title: 'Rejections', icon: 'block', description: 'View all rejected batches', route: '/rejections', roles: [1, 2, 3, 4] },
  ];

  ngOnInit(): void {
    // Subscribe to the current user observable to get user info
    this.auth.currentUser$.subscribe(user => this.user = user);
  }

  /** Check if the current user's role can see a card */
  canSee(roles: number[]): boolean {
    const roleId = this.user?.roleId;
    return roleId ? roles.includes(roleId) : false;
  }
}
