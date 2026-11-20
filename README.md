# Ecom — .NET 8 E-commerce API

E-commerce Web API built with **ASP.NET Core 8**, the repository pattern and Unit of Work. Pairs with the Angular client in [Ecom-Angular-With-DotNetApi](https://github.com/Ahmadjarad47/Ecom-Angular-With-DotNetApi).

## Features

- **Catalog** — products, categories, photos, ratings, filtering and paging
- **Basket** — customer basket stored in **Redis**
- **Orders** — orders, delivery methods, shipping address, order status
- **Payments** — **Stripe** integration
- **Accounts** — ASP.NET Core Identity, **JWT** authentication, email confirmation (MailKit)
- **API docs** — Swagger

## Structure

```
Ecom.API              Controllers (Account, Baskets, Categories, Orders, Payments, Products, Ratings, ...)
Ecom.Core             Entities, DTOs, interfaces, services contracts
Ecom.infrastructure   EF Core (SQL Server), repositories, Unit of Work, Redis, email, Stripe, JWT
```

## Tech Stack

.NET 8 · Entity Framework Core (SQL Server) · ASP.NET Core Identity · JWT · Redis · Stripe · AutoMapper · MailKit · Swagger

## Getting Started

**Prerequisites:** .NET 8 SDK, SQL Server, Redis.

```bash
git clone https://github.com/Ahmadjarad47/Ecom-App-.NET-8-With-Angular.git
cd Ecom-App-.NET-8-With-Angular
# set connection strings, JWT, Stripe and SMTP settings in appsettings.Development.json
dotnet restore
dotnet ef database update --project Ecom.infrastructure --startup-project Ecom.API
dotnet run --project Ecom.API
```

## Author

**Ahmad Jarad** — [GitHub](https://github.com/Ahmadjarad47)