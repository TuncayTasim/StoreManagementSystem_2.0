// --- Inventory models matching Warehouse, Shelf, and related entities ---

export interface Warehouse {
  id: number;
  actionId: number;
  actionType?: ActionType;
  productId: number;
  product?: { id: number; name: string; barcode: string };
  quantity: number;
  currentQuantity: number;
  actionDateTime: string;
  restockDetails?: WarehouseRestock;
}

export interface WarehouseRestock {
  id: number;
  warehouseId: number;
  priceBought: number;
  expirationDate: string;
}

export interface Shelf {
  id: number;
  actionId: number;
  actionType?: ActionType;
  productId: number;
  product?: { id: number; name: string; barcode: string };
  quantity: number;
  currentQuantity: number;
  actionDateTime: string;
  restockDetails?: ShelfRestock;
}

export interface ShelfRestock {
  id: number;
  shelfId: number;
  priceSell: number;
}

export interface ActionType {
  actionTypeId: number;
  name: string;
}

export interface Rejection {
  id: number;
  sourceType: string;
  productName?: string;
  reason: string;
  rejectedAt: string;
}

export interface RejectionDTO {
  id: number;
  reason: string;
}
