import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [RouterLink, MatButtonModule, MatIconModule],
  template: `
    <div class="not-found">
      <mat-icon class="big-icon">search_off</mat-icon>
      <h1>404</h1>
      <p>Page not found</p>
      <a mat-raised-button color="primary" routerLink="/dashboard">Go to Dashboard</a>
    </div>
  `,
  styles: [`
    .not-found {
      text-align: center;
      padding: 80px 20px;

      .big-icon {
        font-size: 80px;
        width: 80px;
        height: 80px;
        color: #ccc;
      }

      h1 {
        font-size: 4em;
        margin: 16px 0 8px;
        color: #333;
      }

      p {
        font-size: 1.2em;
        color: #999;
        margin-bottom: 24px;
      }
    }
  `]
})
export class NotFoundComponent {}
