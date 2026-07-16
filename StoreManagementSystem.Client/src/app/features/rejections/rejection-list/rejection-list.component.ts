import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

// Angular Material modules
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { InventoryService } from '../../../core/services/inventory.service';
import { Rejection } from '../../../core/models/inventory.model';

/**
 * RejectionListComponent
 * -----------------------
 * Displays all rejection records in a read-only table.
 * Data is fetched from InventoryService.getRejections().
 * Shows Source Type, Source ID, Reason, and timestamp.
 */
@Component({
  selector: 'app-rejection-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './rejection-list.component.html',
  styleUrls: ['./rejection-list.component.scss']
})
export class RejectionListComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private snackBar = inject(MatSnackBar);

  /** Rejection records displayed in the table */
  rejections: Rejection[] = [];

  /** Column definitions for the rejections table */
  displayedColumns: string[] = [
    'id', 'sourceType', 'productName', 'reason', 'rejectedAt'
  ];

  ngOnInit(): void {
    this.loadRejections();
  }

  /** Fetch all rejection records from the backend */
  loadRejections(): void {
    this.inventoryService.getRejections().subscribe({
      next: (data) => (this.rejections = data),
      error: () => this.snackBar.open('Failed to load rejections', 'Close', { duration: 3000 })
    });
  }
}
