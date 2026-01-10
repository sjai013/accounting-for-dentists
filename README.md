# Accounting for Dentists

A secure, multi-tenant ASP.NET Core web application to help dentists and other solo practitioners track income, expenses, and tax obligations.

---

## 🦷 Overview

**Accounting for Dentists** is a .NET web application built to simplify bookkeeping and tax preparation for dental practices and similar small businesses. The application focuses on clean data separation between users (tenants), secure authentication, and practical reporting for tax workflows.

The project is implemented using **ASP.NET Core (minimal hosting model)** with **Entity Framework Core** and is designed to run either locally or in containers.

---

## ✨ Key Features

* Multi-tenant architecture with **separate encrypted databases per user**
* Income and expense tracking
* Attachment support for receipts and documents
* Authentication via **Microsoft OpenID Connect**
* Entity Framework Core with SQLite
* Minimal-API based ASP.NET Core application
* Ready for containerized deployment

---

## 🏗 Architecture

* **ASP.NET Core Web App** (minimal APIs)
* **Entity Framework Core** for data access
* **SQLite** databases stored per tenant
* **OpenID Connect** authentication
* Strong separation between infrastructure, domain models, and endpoints

Tenant data is stored under a `tenants/` directory, with one database per authenticated user. Encryption keys are derived from the authenticated user identity.

---

## 🛠 Tech Stack

* **.NET** (ASP.NET Core Web SDK)
* **C#** (preview language features enabled)
* **Entity Framework Core**
* **SQLite**
* **Microsoft Authentication (OIDC)**
* **HTML / CSS / Static Assets** for UI

---

## 🚀 Getting Started

### Prerequisites

* .NET SDK (matching the version specified in the `.csproj`)
* (Optional) Docker

### Clone the Repository

```bash
git clone https://github.com/sjai013/accounting-for-dentists.git
cd accounting-for-dentists
```

### Run Locally

```bash
dotnet restore
dotnet run
```

The application will start on the configured local development port and redirect to the login flow when accessed.

---

## 🔐 Authentication Configuration

Authentication is configured using **Microsoft OpenID Connect**. The following configuration values must be provided via environment variables, user secrets, or `appsettings.json`:

```json
"Authentication": {
  "Microsoft": {
    "ClientId": "<client-id>",
    "ClientSecret": "<client-secret>"
  }
}
```

> Client secrets should **never** be committed to source control.

---

## 🗄 Data Storage & Multi-Tenancy

* Each authenticated user is assigned a tenant
* Tenant databases are stored separately on disk
* Databases are encrypted using keys derived from the user identity
* EF Core manages schema and migrations

This design ensures strong isolation between users while keeping deployment simple.

---

## 🐳 Docker (Optional)

If you wish to containerize the application:

```bash
docker build -t accounting-for-dentists .
docker run -p 8080:8080 accounting-for-dentists
```

Adjust ports and environment variables as required.

---

## 📁 Project Structure (Simplified)

```
/AccountingForDentists
  Program.cs           # Application bootstrap and endpoints
  *.cs                 # Domain, data, and service logic
/wwwroot               # Static assets (CSS, images)
/appsettings.json      # Configuration
```

---

## 🤝 Contributing

Contributions are welcome.

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Open a pull request

---

## 📄 License

This project is licensed under the **MIT License**. See the `LICENSE` file for details.

---

## 👤 Author

**Sahil Jain**

Built to support real-world tax and accounting workflows for dental practices and other professionals.
