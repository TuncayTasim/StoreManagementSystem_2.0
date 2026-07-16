import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

// Angular Material modules
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
 * SellComponent
 * --------------
 * Provides a form to process a sale from the shelf.
 * Collects productId, quantity, and paymentMethod (Cash, Card, Online),
 * then calls InventoryService.sell().
 */
@Component({
  selector: 'app-sell',
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
  templateUrl: './sell.component.html',
  styleUrls: ['./sell.component.scss']
})
export class SellComponent implements OnInit {
  private fb = inject(FormBuilder);
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Reactive form for the sell action */
  sellForm!: FormGroup;

  /** Products available for selection */
  products: Product[] = [];

  /** Available payment methods — used in the dropdown */
  paymentMethods: string[] = ['Cash', 'Card', 'Online'];

  ngOnInit(): void {
    // Build form with all required fields
    this.sellForm = this.fb.group({
      productId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(1)]],
      paymentMethod: [null, Validators.required]
    });

    // Load products for the dropdown selector
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** Process the sale by submitting the form data to the backend */
  onSubmit(): void {
    if (this.sellForm.invalid) return;

    const { productId, quantity, paymentMethod } = this.sellForm.value;

    this.inventoryService.sell(productId, quantity, paymentMethod).subscribe({
      next: (msg) => {
        this.snackBar.open(msg || 'Sale processed successfully!', 'Close', { duration: 3000 });
        this.sellForm.reset();
      },
      error: (err) => {
        this.snackBar.open(err.error || 'Sale failed', 'Close', { duration: 3000 });
      }
    });
  }
}
