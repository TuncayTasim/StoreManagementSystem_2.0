import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Warehouse, Shelf, Rejection, RejectionDTO } from '../models/inventory.model';

@Injectable({ providedIn: 'root' })
export class InventoryService {
  private apiUrl = `${environment.apiUrl}/inventory`;

  constructor(private http: HttpClient) {}

  // ─── Warehouse ──────────────────────────────────────

  /** Restock a product into the warehouse. */
  restock(productId: number, quantity: number, price: number, daysToExpire: number): Observable<string> {
    return this.http.post(
      `${this.apiUrl}/restock?productId=${productId}&quantity=${quantity}&price=${price}&daysToExpire=${daysToExpire}`,
      {},
      { responseType: 'text' }
    );
  }

  /** Get all warehouse batches for a specific product. */
  getWarehouseBatches(productId: number): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(`${this.apiUrl}/warehouse/batches/${productId}`);
  }

  /** Get warehouse audit history, optionally filtered by product. */
  getWarehouseHistory(productId?: number): Observable<Warehouse[]> {
    const params = productId ? `?productId=${productId}` : '';
    return this.http.get<Warehouse[]>(`${this.apiUrl}/warehouse/history${params}`);
  }

  /** Reject a warehouse batch with a reason. */
  rejectWarehouse(dto: RejectionDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/reject/warehouse`, dto, { responseType: 'text' });
  }

  // ─── Shelf ──────────────────────────────────────────

  /** Move a product from warehouse to shelf. */
  moveToShelf(productId: number, quantity: number, sellPrice: number): Observable<string> {
    return this.http.post(
      `${this.apiUrl}/move-to-shelf?productId=${productId}&quantity=${quantity}&sellPrice=${sellPrice}`,
      {},
      { responseType: 'text' }
    );
  }

  /** Get all shelf batches for a specific product. */
  getShelfBatches(productId: number): Observable<Shelf[]> {
    return this.http.get<Shelf[]>(`${this.apiUrl}/shelf/batches/${productId}`);
  }

  /** Get shelf audit history, optionally filtered by product. */
  getShelfHistory(productId?: number): Observable<Shelf[]> {
    const params = productId ? `?productId=${productId}` : '';
    return this.http.get<Shelf[]>(`${this.apiUrl}/shelf/history${params}`);
  }

  /** Reject a shelf batch with a reason. */
  rejectShelf(dto: RejectionDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/reject/shelf`, dto, { responseType: 'text' });
  }

  // ─── Sales (via Inventory controller) ───────────────

  /** Process a sale from the shelf. */
  sell(productId: number, quantity: number, paymentMethod: string): Observable<string> {
    return this.http.post(
      `${this.apiUrl}/sell?productId=${productId}&quantity=${quantity}&paymentMethod=${paymentMethod}`,
      {},
      { responseType: 'text' }
    );
  }

  // ─── Rejections ─────────────────────────────────────

  /** Get all rejection records. */
  getRejections(): Observable<Rejection[]> {
    return this.http.get<Rejection[]>(`${this.apiUrl}/rejections`);
  }
}
