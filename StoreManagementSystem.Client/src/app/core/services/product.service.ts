import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  /** Fetch all products from the API. */
  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  /** Fetch a single product by its ID. */
  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  /** Create a new product. The backend auto-generates the barcode. */
  create(product: Partial<Product>): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  /** Update an existing product. */
  update(id: number, product: Product): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, product);
  }

  /** Delete a product by ID. */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
