// --- Product domain models matching backend entities ---

export interface Product {
  id: number;
  name: string;
  description: string;
  categoryId: number;
  category?: Category;
  supplierId: number;
  supplier?: Supplier;
  sku: string;
  barcode: string;
  warehouseQuantity: number;
  shelfQuantity: number;
}

export interface Category {
  categoryId: number;
  name: string;
}

export interface Supplier {
  supplierId: number;
  name: string;
}
