# 🏪 StoreManagementSystem 2.0

A full-stack store and inventory management system designed to handle the complete product lifecycle — from warehouse restocking through shelf placement to point-of-sale transactions. Built with a **.NET 10 Web API** backend and an **Angular 19** frontend using **Angular Material**.

---

## 📋 Table of Contents

- [Purpose](#-purpose)
- [Key Features](#-key-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Docker Deployment](#-docker-deployment)
- [API Endpoints](#-api-endpoints)
- [Testing](#-testing)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Purpose

Managing inventory across multiple departments (warehouse, shelves, and sales) is complex. StoreManagementSystem 2.0 solves this by providing:

- **A unified platform** where every product is tracked from the moment it arrives in the warehouse, through its placement on store shelves, to its final sale.
- **Role-based workflows** so that warehouse managers, shelf managers, sales managers, and admins each see only the tools relevant to their responsibilities.
- **Audit trails** for every inventory operation, making it easy to trace stock movements and identify discrepancies.
- **Automated barcode generation** (EAN-13, prefix `380`) so new products are immediately scannable and trackable.

Whether you're running a small retail shop or learning full-stack development, this project demonstrates real-world patterns for inventory management, authentication, and role-based access control.

---

## ✨ Key Features

### 🔐 Authentication & Security
- **User Registration** with email confirmation via SMTP (MailKit)
- **JWT Bearer Authentication** with role-based access control
- **BCrypt Password Hashing** for secure credential storage
- **Forgot/Reset Password** flow with secure token-based email verification
- **Change Password** functionality for authenticated users
- **Four user roles**: Admin, Shelf Manager, Warehouse Manager, Sales Manager

### 📦 Inventory Pipeline
The system models a real-world inventory flow:

```
Supplier → Warehouse → Shelf → Sale
                ↘         ↘
              Rejection  Rejection
```

- **Warehouse Restocking** — receive bulk inventory from suppliers with batch tracking
- **Shelf Management** — move products from warehouse to shelves for direct sale
- **Batch Tracking** — view and manage individual batches in both warehouse and shelf
- **Rejection System** — reject defective or expired batches from either warehouse or shelf with a recorded reason

### 🏷️ Product Management
- **CRUD operations** for products with category and supplier associations
- **Automatic EAN-13 Barcode Generation** (prefix `380`) for every new product
- **Product catalogue** accessible across all departments

### 💰 Sales System
- **Record sales** from shelf inventory with automatic quantity deduction
- **Sales History** with date/time, product details, quantities, and payment method
- **Absolute Total** — a live-calculated running total across all transactions
- **Filterable history** by product for targeted reporting

### 📊 Audit & History
- **Departmental logs** for Warehouse and Shelf operations
- **Filterable** audit trails by product name
- **Full rejection history** with timestamps and reasons

### 📱 Modern Frontend
- **Angular 19** with standalone components and lazy-loaded routes
- **Angular Material** design system for a polished, responsive UI
- **Role-aware navigation** — menus and pages adapt to the logged-in user's permissions
- **Auth guards** protecting all dashboard and management routes

---

## 🏗️ Architecture

The application follows a **clean 3-layer architecture** on the backend:

```
┌─────────────────────────────────────────────────────┐
│                   Angular 19 Client                 │
│          (Angular Material · Lazy Loading)           │
└───────────────────────┬─────────────────────────────┘
                        │  HTTP / JWT
┌───────────────────────▼─────────────────────────────┐
│                   .NET 10 Web API                   │
│  ┌──────────────┐  ┌────────────┐  ┌─────────────┐ │
│  │  Controllers │→ │  Services  │→ │Repositories │ │
│  └──────────────┘  └────────────┘  └──────┬──────┘ │
│  ┌──────────────┐  ┌────────────┐         │        │
│  │     DTOs     │  │  Helpers   │         │        │
│  └──────────────┘  └────────────┘         │        │
└───────────────────────────────────┬───────┘────────┘
                                    │  EF Core
                        ┌───────────▼──────────┐
                        │   SQL Server (SSMS)  │
                        └──────────────────────┘
```

- **Controllers** — thin HTTP endpoints, no business logic
- **Services** — business logic layer with interface abstractions
- **Repositories** — data access via EF Core with interface abstractions
- **Helpers** — static utility classes (barcode generation, email sending)
- **DTOs** — data transfer objects to isolate API contracts from entities

---

## 🛠️ Tech Stack

| Layer | Technology | Version |
|---|---|---|
| **Backend API** | .NET Web API (C#) | .NET 10 |
| **Frontend** | Angular (TypeScript) | 19.x |
| **UI Framework** | Angular Material + CDK | 19.x |
| **Database** | SQL Server (LocalDB / Docker) | 2022 |
| **ORM** | Entity Framework Core (Code-First) | 10.x |
| **Authentication** | JWT Bearer Tokens | — |
| **Password Hashing** | BCrypt.Net-Next | 4.1.0 |
| **Email** | MailKit (SMTP) | 4.15.1 |
| **Testing** | xUnit + Moq | 2.9 / 4.20 |
| **Containerisation** | Docker + Docker Compose | Multi-stage build |
| **API Documentation** | Scalar (OpenAPI) | — |

---

## 📁 Project Structure

```
StoreManagementSystem_2.0/
│
├── StoreManagementSystem.slnx               # Solution file
├── Dockerfile                               # Multi-stage Docker build (Angular + .NET)
├── docker-compose.yml                       # SQL Server + App orchestration
│
├── StoreManagementSystem.API/               # ── Backend (.NET 10 Web API) ──
│   ├── Program.cs                           # Entry point, DI registration, middleware
│   ├── appsettings.json                     # Connection strings, JWT key, email config
│   ├── appsettings.Docker.json              # Docker-specific overrides
│   ├── Controllers/
│   │   ├── AuthController.cs                # Register, Login, Email confirm, Password reset
│   │   ├── ProductsController.cs            # Product CRUD
│   │   ├── InventoryController.cs           # Warehouse, Shelf, Rejections
│   │   └── SalesController.cs               # Sales recording & history
│   ├── Services/                            # Business logic (interfaces + implementations)
│   │   ├── AuthService.cs
│   │   ├── ProductService.cs
│   │   ├── WarehouseService.cs
│   │   ├── ShelfService.cs
│   │   ├── SalesService.cs
│   │   └── RejectionService.cs
│   ├── Repositories/                        # Data access layer
│   ├── Interfaces/                          # Service & repository contracts
│   ├── DTOs/                                # Data Transfer Objects
│   ├── Data/
│   │   └── StoreDbContext.cs                # DbContext with Fluent API configuration
│   ├── Helpers/
│   │   ├── BarcodeGenerator.cs              # EAN-13 barcode generation
│   │   └── EmailSender.cs                   # SMTP email via MailKit
│   ├── Migrations/                          # EF Core migration history
│   └── wwwroot/                             # Legacy static frontend (HTML/JS/Bootstrap)
│
├── StoreManagementSystem.Client/            # ── Frontend (Angular 19) ──
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/                        # Guards, interceptors, models, services
│   │   │   ├── features/                    # Feature modules:
│   │   │   │   ├── auth/                    #   Login, Register, Confirm Email, Passwords
│   │   │   │   ├── dashboard/               #   Main dashboard
│   │   │   │   ├── products/                #   Product list & management
│   │   │   │   ├── warehouse/               #   Restock, batches, history
│   │   │   │   ├── shelf/                   #   Move to shelf, batches, history
│   │   │   │   ├── sales/                   #   Sell, sales history
│   │   │   │   └── rejections/              #   Rejection list
│   │   │   ├── shared/                      # Shared components (e.g., 404 page)
│   │   │   ├── app.routes.ts                # Lazy-loaded route definitions
│   │   │   └── app.config.ts                # App-level providers
│   │   └── environments/                    # Environment configs (dev / prod)
│   └── package.json
│
├── StoreManagementSystem.Tests/             # ── Unit Tests (xUnit + Moq) ──
│   ├── AuthServiceTests.cs
│   ├── BarcodeServiceTests.cs
│   ├── ProductServiceTests.cs
│   ├── SalesServiceTests.cs
│   ├── ShelfServiceTests.cs
│   └── WarehouseServiceTests.cs
│
└── .github/workflows/                       # CI/CD (GitHub Actions)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/) and npm
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Angular CLI](https://angular.dev/tools/cli) (`npm install -g @angular/cli`)

### 1. Clone the Repository

```bash
git clone https://github.com/TuncayTasim/StoreManagementSystem_2.0.git
cd StoreManagementSystem_2.0
```

### 2. Configure the Database

Update `StoreManagementSystem.API/appsettings.json` with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=StoreManagementSystem;Trusted_Connection=True;"
}
```

Apply EF Core migrations:

```powershell
dotnet ef database update --project StoreManagementSystem.API
```

### 3. Configure Email (SMTP)

Update `appsettings.json` with your Gmail credentials:

```json
"EmailSettings": {
  "FromEmail": "your-email@gmail.com",
  "Password": "xxxx xxxx xxxx xxxx"
}
```

> **Note:** The password field requires a **Google App Password**, not your regular Gmail password. See [App Password Google](https://support.google.com/accounts/answer/185833) for setup instructions.

### 4. Run the Backend

```powershell
dotnet run --project StoreManagementSystem.API/StoreManagementSystem.API.csproj
```

The API will be available at:
- **HTTPS:** `https://localhost:7274`
- **HTTP:** `http://localhost:5081`
- **API Docs (Scalar):** `https://localhost:7274/scalar`

### 5. Run the Frontend

```powershell
cd StoreManagementSystem.Client
npm install
npm start
```

The Angular app will be available at `http://localhost:4200` and will proxy API requests to the backend.

---

## 🐳 Docker Deployment

Run the entire application stack (SQL Server + API + Angular) with a single command:

```powershell
docker-compose up --build
```

This will:
1. **Build the Angular frontend** in production mode (Node 22 Alpine)
2. **Build the .NET API** and publish in Release mode (.NET 10 SDK)
3. **Start SQL Server 2022** with persistent volume storage
4. **Serve the application** at `http://localhost:8080`

The multi-stage Dockerfile ensures the final runtime image is lightweight (ASP.NET runtime only), with the compiled Angular static files served from the API's `wwwroot/` folder.

> **Tip:** Database data persists across container restarts via the `sqlserver-data` Docker volume.

---

## 📡 API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Login and receive JWT token |
| `GET` | `/api/auth/confirm-email` | Confirm email with token |
| `POST` | `/api/auth/forgot-password` | Send password reset email |
| `POST` | `/api/auth/reset-password` | Reset password with token |

### Products (`/api/products`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/products` | Get all products |
| `POST` | `/api/products` | Create a new product (auto-generates barcode) |
| `PUT` | `/api/products/{id}` | Update a product |
| `DELETE` | `/api/products/{id}` | Delete a product |

### Inventory (`/api/inventory`)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/inventory/warehouse/restock` | Restock a product in the warehouse |
| `GET` | `/api/inventory/warehouse/batches` | View warehouse batches |
| `GET` | `/api/inventory/warehouse/history` | View warehouse operation history |
| `POST` | `/api/inventory/shelf/move` | Move stock from warehouse to shelf |
| `GET` | `/api/inventory/shelf/batches` | View shelf batches |
| `GET` | `/api/inventory/shelf/history` | View shelf operation history |
| `POST` | `/api/inventory/reject` | Reject a batch with a reason |

### Sales (`/api/sales`)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/sales` | Record a sale |
| `GET` | `/api/sales/history` | View sales history |
| `GET` | `/api/sales/total` | Get the absolute total of all sales |

> **Note:** All endpoints except authentication routes require a valid JWT token in the `Authorization: Bearer <token>` header. Some endpoints are further restricted by user role.

---

## 🧪 Testing

The project includes comprehensive unit tests for the service layer using **xUnit** and **Moq**.

### Run All Tests

```powershell
dotnet test
```

### Test Coverage

| Test File | Service Tested | What's Covered |
|---|---|---|
| `AuthServiceTests.cs` | AuthService | Registration, login, password hashing |
| `BarcodeServiceTests.cs` | BarcodeGenerator | EAN-13 barcode generation & validation |
| `ProductServiceTests.cs` | ProductService | CRUD operations |
| `SalesServiceTests.cs` | SalesService | Sale recording, quantity deduction |
| `ShelfServiceTests.cs` | ShelfService | Move-to-shelf, shelf batches |
| `WarehouseServiceTests.cs` | WarehouseService | Restocking, warehouse batches |

### Testing Approach

- **Pattern:** Arrange / Act / Assert
- **Mocking:** Repository interfaces are mocked with Moq; services are tested in isolation
- **Naming convention:** `MethodName_ExpectedBehaviour` (e.g., `GetAllProductsAsync_ReturnsAllProducts`)

---

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

Please ensure all unit tests pass (`dotnet test`) before submitting a PR.

---

## 📄 License

This project is for educational and portfolio purposes.
