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
 * ShelfHistoryComponent
 * ----------------------
 * Displays the audit history for shelf actions.
 * Same pattern as WarehouseHistoryComponent, but uses
 * InventoryService.getShelfHistory() instead.
 */
@Component({
  selector: 'app-shelf-history',
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
  templateUrl: './shelf-history.component.html',
  styleUrls: ['./shelf-history.component.scss']
})
export class ShelfHistoryComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Products for the optional filter dropdown */
  products: Product[] = [];

  /** Selected product ID (null = show all) */
  selectedProductId: number | null = null;

  /** Shelf history records */
  history: Shelf[] = [];

  /** Columns to display in the table */
  displayedColumns: string[] = [
    'productName', 'actionType', 'quantity', 'currentQuantity', 'actionDateTime'
  ];

  ngOnInit(): void {
    // Load products for filter
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });

    // Load all shelf history on init
    this.loadHistory();
  }

  /** Called when the product filter selection changes */
  onProductChange(productId: number | null): void {
    this.selectedProductId = productId;
    this.loadHistory();
  }

  /** Fetch shelf history, optionally filtered by product */
  loadHistory(): void {
    this.inventoryService.getShelfHistory(this.selectedProductId ?? undefined).subscribe({
      next: (data) => (this.history = data),
      error: () => this.snackBar.open('Failed to load shelf history', 'Close', { duration: 3000 })
    });
  }
}
