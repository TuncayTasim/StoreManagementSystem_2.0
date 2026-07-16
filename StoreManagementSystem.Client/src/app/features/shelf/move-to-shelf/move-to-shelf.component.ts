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
 * MoveToShelfComponent
 * ---------------------
 * Provides a form to move products from the warehouse to the shelf.
 * Collects productId, quantity, and sellPrice, then calls
 * InventoryService.moveToShelf().
 */
@Component({
  selector: 'app-move-to-shelf',
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
  templateUrl: './move-to-shelf.component.html',
  styleUrls: ['./move-to-shelf.component.scss']
})
export class MoveToShelfComponent implements OnInit {
  private fb = inject(FormBuilder);
  private inventoryService = inject(InventoryService);
  private productService = inject(ProductService);
  private snackBar = inject(MatSnackBar);

  /** Reactive form for the move-to-shelf action */
  moveForm!: FormGroup;

  /** Products available for selection */
  products: Product[] = [];

  ngOnInit(): void {
    // Build form with validation — sellPrice is the price the item will be sold at
    this.moveForm = this.fb.group({
      productId: [null, Validators.required],
      quantity: [null, [Validators.required, Validators.min(1)]],
      sellPrice: [null, [Validators.required, Validators.min(0.01)]]
    });

    // Load all products for the dropdown
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** Submit the form to move stock from warehouse to shelf */
  onSubmit(): void {
    if (this.moveForm.invalid) return;

    const { productId, quantity, sellPrice } = this.moveForm.value;

    this.inventoryService.moveToShelf(productId, quantity, sellPrice).subscribe({
      next: (msg) => {
        this.snackBar.open(msg || 'Moved to shelf successfully!', 'Close', { duration: 3000 });
        this.moveForm.reset();
      },
      error: (err) => {
        this.snackBar.open(err.error || 'Move to shelf failed', 'Close', { duration: 3000 });
      }
    });
  }
}
