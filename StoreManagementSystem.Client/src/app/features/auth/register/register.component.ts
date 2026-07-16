import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

import { AuthService } from '../../../core/services/auth.service';
import { UserRegisterDTO } from '../../../core/models/user.model';

/**
 * RegisterComponent — standalone component for new user registration.
 * Demonstrates MatSelect for role selection and multi-field reactive forms.
 */
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule,
    MatSelectModule, // required for the role dropdown
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  private auth = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  hidePassword = true;

  /** Available roles exposed to the template for mat-select options */
  roles = [
    { id: 1, name: 'Admin' },
    { id: 2, name: 'Shelf Manager' },
    { id: 3, name: 'Warehouse Manager' },
    { id: 4, name: 'Sales Manager' },
  ];

  /**
   * FormGroup with one FormControl per registration field.
   * Validators.required marks fields that must be filled before submission.
   */
  registerForm = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    roleId: new FormControl<number | null>(null, [Validators.required]),
  });

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    const dto: UserRegisterDTO = this.registerForm.getRawValue() as UserRegisterDTO;

    this.auth.register(dto).subscribe({
      next: () => {
        this.snackBar.open('Registration successful! Check your email.', 'Close', {
          duration: 5000,
        });
        this.router.navigate(['/login']);
      },
      error: (err) => {
        const message = err?.error?.message || 'Registration failed. Please try again.';
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
