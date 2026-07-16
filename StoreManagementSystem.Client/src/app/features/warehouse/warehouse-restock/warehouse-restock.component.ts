import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

// Angular Material modules imported directly for standalone component
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

import { InventoryService } from '../../../core/services/inventory.service';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';

/**
 * WarehouseRestockComponent
 * -------------------------
 * Allows the user to restock a product into the warehouse.
 * Uses a reactive form to collect productId, quantity, price, and daysToExpire,
 * then calls InventoryService.restock() on submission.
 */
@Component({
  selector: 'app-warehouse-restock',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatSnackBarModule,
    MatIconModule
  ],
  templateUrl: './warehouse-restock.component.html',
  styleUrls: ['./warehouse-restock.component.scss']
})
export class WarehouseRestockComponent implements OnInit {
  // inject() is the modern Angular way to obtain service instances (no constructor injection needed)
  private fb = inject(FormBuilder);
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Reactive form group for the restock form */
  restockForm!: FormGroup;

  /** Products list used to populate the product selector dropdown */
  products: Product[] = [];

  ngOnInit(): void {
    // Build the reactive form with validation rules
    this.restockForm = this.fb.group({
      productId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(1)]],
      price: [null, [Validators.required, Validators.min(0.01)]],
      daysToExpire: [null, [Validators.required, Validators.min(1)]]
    });

    // Load all products for the dropdown selector
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** Submit the restock form to the backend */
  onSubmit(): void {
    if (this.restockForm.invalid) return;

    const { productId, quantity, price, daysToExpire } = this.restockForm.value;

    this.inventoryService.restock(productId, quantity, price, daysToExpire).subscribe({
      next: (msg) => {
        // Show success notification and reset the form for the next entry
        this.snackBar.open(msg || 'Restock successful!', 'Close', { duration: 3000 });
        this.restockForm.reset();
      },
      error: (err) => {
        this.snackBar.open(err.error || 'Restock failed', 'Close', { duration: 3000 });
      }
    });
  }
}
