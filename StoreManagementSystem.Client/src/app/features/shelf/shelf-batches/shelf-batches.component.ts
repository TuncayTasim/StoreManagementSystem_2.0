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
import { Shelf } from '../../../core/models/inventory.model';

/**
 * ShelfBatchesComponent
 * ----------------------
 * Displays shelf batches for a selected product in a table.
 * Same pattern as WarehouseBatchesComponent, but uses shelf-specific
 * service methods (getShelfBatches, rejectShelf).
 */
@Component({
  selector: 'app-shelf-batches',
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
  templateUrl: './shelf-batches.component.html',
  styleUrls: ['./shelf-batches.component.scss']
})
export class ShelfBatchesComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Products for the filter dropdown */
  products: Product[] = [];

  /** Currently selected product */
  selectedProductId: number | null = null;

  /** Shelf batch data for the table */
  batches: Shelf[] = [];

  /** Table column definitions */
  displayedColumns: string[] = [
    'id', 'quantity', 'currentQuantity', 'actionDateTime', 'sellPrice', 'actions'
  ];

  ngOnInit(): void {
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** When user selects a product, load its shelf batches */
  onProductChange(productId: number): void {
    this.selectedProductId = productId;
    this.loadBatches();
  }

  /** Fetch shelf batches from the backend */
  loadBatches(): void {
    if (!this.selectedProductId) return;

    this.inventoryService.getShelfBatches(this.selectedProductId).subscribe({
      next: (data) => (this.batches = data),
      error: () => this.snackBar.open('Failed to load shelf batches', 'Close', { duration: 3000 })
    });
  }

  /**
   * Reject a shelf batch — prompts user for a reason via window.prompt(),
   * then calls the rejectShelf API endpoint.
   */
  rejectBatch(batchId: number): void {
    const reason = window.prompt('Enter rejection reason:');
    if (!reason) return;

    this.inventoryService.rejectShelf({ id: batchId, reason }).subscribe({
      next: (msg) => {
        this.snackBar.open(msg || 'Shelf batch rejected', 'Close', { duration: 3000 });
        this.loadBatches(); // Refresh table data
      },
      error: (err) => {
        this.snackBar.open(err.error || 'Rejection failed', 'Close', { duration: 3000 });
      }
    });
  }
}
