# Coding Standards

The **Coder** must adhere to the following architectural and business standards:

## 1. 3-Layer Architecture
Every entity must follow this structure:
- **Repository**: Interface and implementation for EF Core operations.
- **Service**: Business logic, validation, and complex data manipulation.
- **Controller**: Endpoint definition and DTO handling.

## 2. Inventory Logic
- **Quantity Updates**: Never update the `Product` quantity without creating a corresponding entry in the `Warehouse` or `Shelf` action tables.
- **CurrentQuantity**: The `CurrentQuantity` field in action tables must match the `Product` quantity *after* the action is applied.
- **Movements**: A "Move to Shelf" action must record a negative quantity in the Warehouse table and a positive quantity in the Shelf table.

## 6. Sales & Reporting
- **Sales History**: Fetch sales records including nested `Shelf`, `Product`, and `Category` data. Order history by `Shelf.ActionDateTime`.
- **Filtering**: All history endpoints (Sales, Warehouse, Shelf) must support an optional `productId` query parameter for filtering.
- **Absolute Total**: Calculate the sum of all individual sale totals `(QuantitySold * PriceSold)` in the UI to provide the absolute total across all transactions.

## 7. Security & Authentication
- Use `BCrypt.Net-Next` for all password hashing.
- Passwords must never be returned in API responses.
- **Roles**: Admin (1), Shelf Manager (2), Warehouse Manager (3), Sales Manager (4).
- **RBAC**: Apply `[Authorize]` to all management controllers. Restrict specific log views and inventory operations based on the user's role.
- **Email Activation & Password Reset**: Use a generated 8-character token stored in `ActionToken`. The same mechanism applies to both new user activation and the "Forgot Password" reset workflow.
- Ensure JWT Bearer authentication is applied to sensitive endpoints, including the user's Role name in the token claims.

## 4. Automation
- Barcodes must be generated in the Service layer using the EAN-13 standard.

## 5. Exception Handling & UI
- **Controllers**: All POST/PUT/DELETE actions must use `try-catch` blocks to handle business logic exceptions (e.g., insufficient stock) and return a `400 BadRequest` with a clear, plain-text error message.
- **Frontend**: Use the global `showErrorAlert` helper in `app_v2.js` to handle non-OK API responses, ensuring users are invited to correct their input. Handle 401 (Unauthorized) and 403 (Forbidden) statuses gracefully.
- **Global Error Page**: Maintain the `UseExceptionHandler` middleware in `Program.cs` as a final safety net.
