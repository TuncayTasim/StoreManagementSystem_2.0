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

import { SalesService } from '../../../core/services/sales.service';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { Sale } from '../../../core/models/sale.model';

/**
 * SalesHistoryComponent
 * ----------------------
 * Displays a table of all sales with an optional product filter.
 * Shows an "Absolute Total" below the table — the sum of
 * (priceSold × quantitySold) across all displayed rows.
 */
@Component({
  selector: 'app-sales-history',
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
  templateUrl: './sales-history.component.html',
  styleUrls: ['./sales-history.component.scss']
})
export class SalesHistoryComponent implements OnInit {
  private salesService = inject(SalesService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Products for the optional filter dropdown */
  products: Product[] = [];

  /** Selected product ID (null = show all sales) */
  selectedProductId: number | null = null;

  /** Sales data displayed in the table */
  sales: Sale[] = [];

  /** Table column definitions */
  displayedColumns: string[] = [
    'productName', 'quantitySold', 'priceSold', 'paymentMethod'
  ];

  ngOnInit(): void {
    // Load products for filter dropdown
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });

    // Load all sales on init
    this.loadSales();
  }

  /** Triggered when the user changes the product filter */
  onProductChange(productId: number | null): void {
    this.selectedProductId = productId;
    this.loadSales();
  }

  /** Fetch sales from the backend, optionally filtered by product */
  loadSales(): void {
    this.salesService.getAll(this.selectedProductId ?? undefined).subscribe({
      next: (data) => (this.sales = data),
      error: () => this.snackBar.open('Failed to load sales', 'Close', { duration: 3000 })
    });
  }

  /**
   * Calculate the absolute total revenue from all displayed sales.
   * Formula: sum of (priceSold × quantitySold) for each row.
   */
  get absoluteTotal(): number {
    return this.sales.reduce(
      (sum, sale) => sum + (sale.priceSold * sale.quantitySold), 0
    );
  }
}
