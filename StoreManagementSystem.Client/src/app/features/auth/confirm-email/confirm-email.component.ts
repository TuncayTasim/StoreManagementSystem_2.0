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

/**
 * ConfirmEmailComponent — standalone component that lets the user
 * submit an email-confirmation token received via email.
 */
@Component({
  selector: 'app-confirm-email',
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
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent {
  private auth = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  /** Single-field form for the confirmation token */
  confirmForm = new FormGroup({
    token: new FormControl('', [Validators.required]),
  });

  onSubmit(): void {
    if (this.confirmForm.invalid) return;

    const token = this.confirmForm.controls.token.value!;

    // confirmEmail() returns Observable<string> — the server's response message
    this.auth.confirmEmail(token).subscribe({
      next: () => {
        this.snackBar.open('Email confirmed!', 'Close', { duration: 5000 });
        this.router.navigate(['/login']);
      },
      error: (err) => {
        const message = err?.error?.message || 'Email confirmation failed.';
        this.snackBar.open(message, 'Close', { duration: 5000 });
      },
    });
  }
}
