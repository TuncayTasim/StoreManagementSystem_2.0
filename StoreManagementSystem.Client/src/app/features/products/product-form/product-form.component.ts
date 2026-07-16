import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Product } from '../../../core/models/product.model';

/**
 * Dialog component for creating or editing a product.
 * 
 * When opened with data (via MAT_DIALOG_DATA), it pre-fills the form = Edit mode.
 * When opened without data, it starts empty = Create mode.
 * 
 * MAT_DIALOG_DATA is an injection token provided by MatDialog.open() — 
 * whatever you pass as `data` in the dialog config gets injected here.
 */
@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent {
  // inject() is the modern way to do DI in Angular standalone components
  private dialogRef = inject(MatDialogRef<ProductFormComponent>);
  
  // MAT_DIALOG_DATA can be null (create mode) or a Product (edit mode)
  // The { optional: true } flag prevents Angular from throwing if no data is provided
  data: Product | null = inject(MAT_DIALOG_DATA, { optional: true }) ?? null;

  isEditMode = !!this.data;

  // Reactive form — each FormControl maps to a form field
  form = new FormGroup({
    name: new FormControl(this.data?.name ?? '', Validators.required),
    description: new FormControl(this.data?.description ?? ''),
    categoryId: new FormControl(this.data?.categoryId ?? 1, Validators.required),
    supplierId: new FormControl(this.data?.supplierId ?? 1, Validators.required),
    sku: new FormControl(this.data?.sku ?? ''),
  });

  onSave(): void {
    if (this.form.valid) {
      // Close the dialog and pass the form value back to the opener
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
