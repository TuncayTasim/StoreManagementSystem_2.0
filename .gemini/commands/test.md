# Testing Standards

The **Tester** must ensure the application's integrity through:

## 1. Service Layer Unit Tests
- Use `xUnit` and `Moq`.
- Focus on quantity calculations (Restock, Move, Sell).
- Verify that `Product` totals match the snapshots in action tables.

## 2. Authentication & Reset Tests
- Verify that `Inactive` users (Status 2) cannot log in.
- Verify that `ConfirmEmailAsync` correctly transitions status to `Active` (Status 1).
- **Password Reset**: Verify that `ForgotPasswordAsync` sends a token and `ResetPasswordAsync` correctly updates the hashed password, then clears the token.

## 3. Error Handling & Validation
- **Negative Testing**: Ensure that invalid inputs (e.g., trying to move 120 kg when 100 kg is available) return a 400 response with a clear message.
- **Frontend Alerting**: Confirm that the `showErrorAlert` correctly parses and displays API error messages.

## 4. Sales & Reporting Tests
- **Sales Data Integrity**: Verify that sales history includes correctly linked `Shelf` and `Product` data.
- **Absolute Total Calculation**: Ensure the sum of `(QuantitySold * PriceSold)` across all sale records matches the displayed **Absolute Total**.

## 5. Barcode Validation
- Ensure generated barcodes follow EAN-13 format and include a correct check digit.
