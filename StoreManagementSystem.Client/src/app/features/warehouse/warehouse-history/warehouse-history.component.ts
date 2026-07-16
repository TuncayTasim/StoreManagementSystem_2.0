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
 * WarehouseHistoryComponent
 * --------------------------
 * Displays the full audit history of warehouse actions.
 * An optional product filter lets the user narrow results to a single product.
 */
@Component({
  selector: 'app-warehouse-history',
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
  templateUrl: './warehouse-history.component.html',
  styleUrls: ['./warehouse-history.component.scss']
})
export class WarehouseHistoryComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Products for the optional filter dropdown */
  products: Product[] = [];

  /** Selected product ID for filtering (null = show all) */
  selectedProductId: number | null = null;

  /** Warehouse history records */
  history: Warehouse[] = [];

  /** Columns displayed in the history table */
  displayedColumns: string[] = [
    'productName', 'actionType', 'quantity', 'currentQuantity', 'actionDateTime'
  ];

  ngOnInit(): void {
    // Load products for the filter dropdown
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });

    // Load all history on init (no filter)
    this.loadHistory();
  }

  /** Triggered when the user changes the product filter */
  onProductChange(productId: number | null): void {
    this.selectedProductId = productId;
    this.loadHistory();
  }

  /** Fetch warehouse history from the backend, optionally filtered by product */
  loadHistory(): void {
    this.inventoryService.getWarehouseHistory(this.selectedProductId ?? undefined).subscribe({
      next: (data) => (this.history = data),
      error: () => this.snackBar.open('Failed to load history', 'Close', { duration: 3000 })
    });
  }
}
