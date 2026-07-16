import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {
  changeForm: FormGroup;
  hideOldPassword = true;
  hideNewPassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.changeForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.changeForm.valid) {
      this.authService.changePassword(this.changeForm.value).subscribe({
        next: (msg: string) => {
          this.snackBar.open(msg || 'Password changed successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/dashboard']);
        },
        error: (err: any) => {
          this.snackBar.open(err.error || 'Failed to change password', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
