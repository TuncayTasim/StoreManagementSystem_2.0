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

import { AuthService } from '../../../core/services/auth.service';
import { UserLoginDTO } from '../../../core/models/user.model';

/**
 * LoginComponent — standalone component handling user authentication.
 * Uses Angular's `inject()` function for dependency injection and
 * Reactive Forms for two-way model binding with validation.
 */
@Component({
  selector: 'app-login',
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
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  // inject() is Angular 14+ shorthand for constructor-based DI
  private auth = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  /** Whether the password field text is hidden */
  hidePassword = true;

  /**
   * Reactive FormGroup — each FormControl maps to an input field.
   * Validators.required ensures the field is non-empty before submission.
   */
  loginForm = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  /** Submit handler — calls AuthService.login() and handles response */
  onSubmit(): void {
    if (this.loginForm.invalid) return;

    // getRawValue() returns typed values including disabled controls
    const dto: UserLoginDTO = this.loginForm.getRawValue() as UserLoginDTO;

    this.auth.login(dto).subscribe({
      next: () => {
        // Navigate to the dashboard on successful login
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        // Check for a specific back-end error code
        const message =
          err?.error?.toString().includes('EMAIL_NOT_CONFIRMED')
            ? 'Please confirm your email before logging in.'
            : err?.error?.message || 'Login failed. Please try again.';

        // MatSnackBar displays a brief notification at the bottom of the screen
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
