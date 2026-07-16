// --- Sale model matching the backend Sale entity ---

export interface Sale {
  id: number;
  shelfId: number;
  shelf?: {
    id: number;
    product?: { id: number; name: string; barcode: string };
    restockDetails?: { priceSell: number };
  };
  priceSold: number;
  quantitySold: number;
  paymentMethod: string;
}
