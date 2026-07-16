import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

// Angular Material modules
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule } from '@angular/material/input';

import { InventoryService } from '../../../core/services/inventory.service';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { Warehouse } from '../../../core/models/inventory.model';

/**
 * WarehouseBatchesComponent
 * --------------------------
 * Displays a table of warehouse batches for a selected product.
 * Each row has a "Reject" action that prompts for a reason and
 * calls InventoryService.rejectWarehouse().
 */
@Component({
  selector: 'app-warehouse-batches',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatInputModule
  ],
  templateUrl: './warehouse-batches.component.html',
  styleUrls: ['./warehouse-batches.component.scss']
})
export class WarehouseBatchesComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Products list for the product filter dropdown */
  products: Product[] = [];

  /** Currently selected product ID */
  selectedProductId: number | null = null;

  /** Warehouse batch data displayed in the table */
  batches: Warehouse[] = [];

  /** Column definitions for the MatTable */
  displayedColumns: string[] = [
    'id', 'quantity', 'currentQuantity', 'actionDateTime',
    'priceBought', 'expirationDate', 'actions'
  ];

  ngOnInit(): void {
    // Load products for the selector
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** Called when user picks a product from the dropdown */
  onProductChange(productId: number): void {
    this.selectedProductId = productId;
    this.loadBatches();
  }

  /** Fetch warehouse batches for the selected product */
  loadBatches(): void {
    if (!this.selectedProductId) return;

    this.inventoryService.getWarehouseBatches(this.selectedProductId).subscribe({
      next: (data) => (this.batches = data),
      error: () => this.snackBar.open('Failed to load batches', 'Close', { duration: 3000 })
    });
  }

  /**
   * Reject a warehouse batch.
   * Uses window.prompt() to collect the rejection reason from the user,
   * then sends it to the backend.
   */
  rejectBatch(batchId: number): void {
    const reason = window.prompt('Enter rejection reason:');
    if (!reason) return; // User cancelled or entered empty string

    this.inventoryService.rejectWarehouse({ id: batchId, reason }).subscribe({
      next: (msg) => {
        this.snackBar.open(msg || 'Batch rejected', 'Close', { duration: 3000 });
        this.loadBatches(); // Refresh the table after rejection
      },
      error: (err) => {
        this.snackBar.open(err.error || 'Rejection failed', 'Close', { duration: 3000 });
      }
    });
  }
}
