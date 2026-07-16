import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { ProductFormComponent } from '../product-form/product-form.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatSnackBarModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  // The columns to display in the Material table
  displayedColumns = ['name', 'description', 'categoryId', 'supplierId', 'sku', 'barcode', 'warehouseQuantity', 'shelfQuantity', 'actions'];
  products: Product[] = [];

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: data => this.products = data,
      error: err => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  /** Open the product form dialog in "create" mode (no data passed) */
  openAddDialog(): void {
    const dialogRef = this.dialog.open(ProductFormComponent, {
      width: '500px'
    });

    // afterClosed() emits the value passed to dialogRef.close()
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.create(result).subscribe({
          next: () => {
            this.snackBar.open('Product created!', 'Close', { duration: 3000 });
            this.loadProducts();
          },
          error: err => this.snackBar.open(err.error || 'Create failed', 'Close', { duration: 3000 })
        });
      }
    });
  }

  /** Open the product form dialog in "edit" mode (pass existing product data) */
  openEditDialog(product: Product): void {
    const dialogRef = this.dialog.open(ProductFormComponent, {
      width: '500px',
      data: product  // This data is injected via MAT_DIALOG_DATA in the dialog component
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const updated = { ...product, ...result };
        this.productService.update(product.id, updated).subscribe({
          next: () => {
            this.snackBar.open('Product updated!', 'Close', { duration: 3000 });
            this.loadProducts();
          },
          error: err => this.snackBar.open(err.error || 'Update failed', 'Close', { duration: 3000 })
        });
      }
    });
  }

  /** Delete a product after confirmation */
  deleteProduct(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.delete(id).subscribe({
        next: () => {
          this.snackBar.open('Product deleted', 'Close', { duration: 3000 });
          this.loadProducts();
        },
        error: err => this.snackBar.open(err.error || 'Delete failed', 'Close', { duration: 3000 })
      });
    }
  }
}
