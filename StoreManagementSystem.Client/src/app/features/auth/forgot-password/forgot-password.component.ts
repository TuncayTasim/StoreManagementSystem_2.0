import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

import { AuthService } from '../../../core/services/auth.service';
import { ForgotPasswordDTO } from '../../../core/models/user.model';

/**
 * ForgotPasswordComponent — standalone component that sends a
 * password-reset token to the user's email address.
 */
@Component({
  selector: 'app-forgot-password',
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
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent {
  private auth = inject(AuthService);
  private snackBar = inject(MatSnackBar);

  /** Form with a single email field */
  forgotForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  onSubmit(): void {
    if (this.forgotForm.invalid) return;

    const dto: ForgotPasswordDTO = this.forgotForm.getRawValue() as ForgotPasswordDTO;

    this.auth.forgotPassword(dto).subscribe({
      next: () => {
        this.snackBar.open('Reset token sent to your email', 'Close', {
          duration: 5000,
        });
      },
      error: (err) => {
        const message = err?.error?.message || 'Request failed. Please try again.';
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
