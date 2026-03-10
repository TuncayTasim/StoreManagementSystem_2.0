# Project Overview: StoreManagementSystem

This document provides a comprehensive overview of the StoreManagementSystem project, including its roles, project details, and core entities.

## Role: Project Orchestrator
The Project Orchestrator manages the development lifecycle using the roles defined in `.gemini/agents/`.

## My Responsibility

As the Project Orchestrator, my primary responsibility is to manage the entire development workflow by adopting specific roles at each phase of the process. I will follow a strict, sequential workflow defined by four key roles, each with its own set of rules and tasks.

## Relationships to Role Files

The workflow is broken down into the following roles, with each file containing specific rules I must follow:

1.  **[Planner](.gemini/commands/plan.md)**: My first role is to be the Planner. I will analyze the request and create a detailed plan. I will not proceed without your approval.
2.  **[Coder](.gemini/commands/code.md)**: After you approve the plan, I will adopt the Coder role to write the code.
3.  **[Tester](.gemini/commands/test.md)**: Once coding is complete, I will become the Tester to write and execute tests to ensure quality.
4.  **[Reviewer](.gemini/commands/review.md)**: Finally, I will act as the Reviewer to perform a final check on all work before completing the task.

## Project Information
*   **Project Name**: StoreManagementSystem
*   **Backend**: .NET 10 Web API (3-Layer Architecture)
*   **Frontend**: HTML5/CSS3 (Bootstrap 5) + Vanilla JavaScript
*   **Database**: SQL Server (EF Core)
*   **Authentication**: JWT Bearer + BCrypt + SMTP Email Activation & Password Reset + Role-Based Access Control (RBAC)

## Implementation Standards

### 1. Architecture
- **Repository Layer**: Direct database access via `StoreDbContext`. Supports optional product-based filtering for history and sales queries.
- **Service Layer**: Core business logic and orchestration of data flow between repositories and controllers.
- **Helper Layer**: Specialized static utilities handling cross-cutting concerns (e.g., `BarcodeGenerator` for EAN-13 creation, `EmailSender` for SMTP dispatch).
- **Controller Layer**: RESTful API endpoints, DTO mapping, and Role-based authorization.

### 2. Business Rules
- **Roles and Permissions**:
    - **Admin (1)**: Unrestricted access to all features, logs, and management.
    - **Shelf Manager (2)**: Management of shelf movements and shelf audit logs.
    - **Warehouse Manager (3)**: Management of warehouse restocking and warehouse audit logs.
    - **Sales Manager (4)**: Management of sales processing and sales history logs.
- **Quantity Tracking**: 
    - `Product` table stores the "Live" state (`WarehouseQuantity`, `ShelfQuantity`).
    - `Warehouse`/`Shelf` tables store the "History" of actions.
    - **Move to Shelf**: Decrements `WarehouseQuantity` and Increments `ShelfQuantity` simultaneously.
- **Filtering**: All audit logs (Sales, Warehouse, Shelf) support filtering by `ProductId`.
- **Barcodes**: Automatically generated using **EAN-13** standard (prefix `380` for Bulgaria).
- **Email Activation & Password Reset**: Users are registered as `Inactive` (Status 2). Activation requires a token sent via SMTP. The same token mechanism is used for the "Forgot Password" workflow to verify ownership before allowing a password reset.
- **Sales History**: Every sale is linked to a `Shelf` action batch. The `ActionDateTime` of the linked shelf record is used as the timestamp for the sale. An **Absolute Total** of all sales is calculated on the frontend by summing up `(QuantitySold * PriceSold)` across all records.

### 3. Error Handling & Feedback
- **API Error Responses**: All endpoints return `400 BadRequest` for input/validation errors, providing a clear and specific error message.
- **Frontend Validation**: Inputs are validated client-side where possible. If an API request fails, the frontend uses a global `showErrorAlert` to notify the user and clear incorrect values (e.g., quantity) for re-entry.
- **Auth Resilience**: Frontend automatically handles session expiration (401) and access denial (403) by prompting for login or notifying the user.
- **Global Resilience**: A global exception handler in the backend ensures the API remains operational and provides generic feedback for unexpected failures.

## Core Entities

### User
- `UserId`, `UserName`, `FirstName`, `LastName`, `RoleId`, `Email`, `PhoneNumber`, `PasswordHash`, `ActionToken`, `StatusId`.
- **Roles**: Admin (1), Shelf Manager (2), Warehouse Manager (3), Sales Manager (4).

### Product
- `Id`, `Name`, `Description`, `CategoryId`, `SupplierId`, `SKU`, `Barcode`, `WarehouseQuantity`, `ShelfQuantity`.

### Inventory Actions (Warehouse & Shelf)
- Records `ActionId`, `ProductId`, `Quantity` (change), and `CurrentQuantity` (snapshot after change).
- `WarehouseRestock` & `ShelfRestock` store price-specific data.
- **Rejection**: Records `Id`, `WarehouseId`, `ShelfId`, `Reason`.
- **ActionTypes**: Restock (1), MoveToShelf (2), Sell (3), Reject (4).

### Sales
- Records `ShelfId`, `PriceSold`, `QuantitySold`, `PaymentMethod`.

