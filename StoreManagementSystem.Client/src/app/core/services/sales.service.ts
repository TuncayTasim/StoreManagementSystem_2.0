import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Sale } from '../models/sale.model';

@Injectable({ providedIn: 'root' })
export class SalesService {
  private apiUrl = `${environment.apiUrl}/sales`;

  constructor(private http: HttpClient) {}

  /** Fetch all sales, optionally filtered by product ID. */
  getAll(productId?: number): Observable<Sale[]> {
    const params = productId ? `?productId=${productId}` : '';
    return this.http.get<Sale[]>(`${this.apiUrl}${params}`);
  }
}
