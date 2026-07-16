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
import { ResetPasswordDTO } from '../../../core/models/user.model';

/**
 * ResetPasswordComponent — standalone component for resetting a user's
 * password using a token obtained from the forgot-password flow.
 */
@Component({
  selector: 'app-reset-password',
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
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent {
  private auth = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  hideNewPassword = true;
  hideConfirmPassword = true;

  /**
   * FormGroup for reset-password — includes token, newPassword,
   * and confirmPassword fields all marked as required.
   */
  resetForm = new FormGroup({
    token: new FormControl('', [Validators.required]),
    newPassword: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });

  onSubmit(): void {
    if (this.resetForm.invalid) return;

    const dto: ResetPasswordDTO = this.resetForm.getRawValue() as ResetPasswordDTO;

    this.auth.resetPassword(dto).subscribe({
      next: () => {
        this.snackBar.open('Password reset successfully', 'Close', {
          duration: 5000,
        });
        this.router.navigate(['/login']);
      },
      error: (err) => {
        const message = err?.error?.message || 'Password reset failed.';
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
