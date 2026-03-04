# StoreManagementSystem

A comprehensive store management application built with a .NET Web API backend and a Bootstrap-based frontend.

## Features

- **User Authentication**: Secure login and registration with hashed passwords. Includes a "Forgot Password" feature allowing users to securely reset their credentials. Users can register as **Admin**, **Shelf Manager**, **Warehouse Manager**, or **Sales Manager**.
- **Role-Based Access Control (RBAC)**: Interface and actions are restricted based on user role (Admin has full access, Managers have access to their respective departments).
- **Email Confirmation & Services**: SMTP integration via `tuncaytasim24@gmail.com` using `MailKit`. Users must confirm their email using a token sent to them. This token mechanism is also used for securely resetting forgotten passwords.
- **Inventory Management**:
    - **Automatic Barcode Generation**: EAN-13 barcodes generated automatically for new products (prefix 380).
    - **Warehouse Tracking**: Manage bulk stock and restocking operations.
    - **Shelf Management**: Track products moved to shelves and available for direct sale.
    - **Reject Inventory Items**: Ability to reject specific batches from both warehouse and shelf with a reason.
    - **Departmental Logs**: Detailed audit logs for Warehouse and Shelf operations, filterable by product.
- **Sales System**: Record transactions and automatically update shelf quantities.
- **Sales History & Absolute Total**: Track all historical sales with date/time, product details, and payment methods. A live "Absolute Total" is calculated across all transactions. History can be filtered by product.

## Tech Stack

- **Backend**: .NET 10 Web API, Entity Framework Core
- **Database**: SQL Server
- **Frontend**: HTML/CSS, Bootstrap 5, Vanilla JavaScript
- **Testing**: xUnit, Moq

## Setup Instructions

1.  **Clone the repository**.
2.  **Configure Database**:
    - Update `appsettings.json` with your SQL Server connection string.
    - Run migrations: `dotnet ef database update --project StoreManagementSystem.API`.
3.  **Configure Email**:
    - Ensure `EmailSettings:Password` in `appsettings.json` contains a valid Gmail App Password for `tuncaytasim24@gmail.com`.
4.  **Run the Application**:
    ```powershell
    dotnet run --project StoreManagementSystem.API/StoreManagementSystem.API.csproj
    ```
5.  **Access**: Open the local URL (e.g., `https://localhost:5001`) in your browser.

## Testing

Run unit tests using:
```powershell
dotnet test
```
