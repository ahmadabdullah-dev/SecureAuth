# SecureAuth With ASP.NET Identity
 Starter API template with layered architecture, built to eliminate repetitive project setup.
 
## Why I Built This
 
As a developer, I noticed I was rewriting the same foundational code for every new project. So I created this repo template to solve that by giving me a clean, reusable base so I can focus on actual feature work instead of re-solving the same problems each time.
 
## Tech Stack 
- **Backend:** .NET, Entity Framework Core (Code-First), PostgreSQL, ASP.NET Identity
## Features
- **Layered architecture:** Data Access => Business => API
- **Result pattern** for consistent success/error handling across services
- **Per-layer dependency injection** (each layer registers its own services)
- **Global usings** and shared constants to reduce boilerplate
- **Authentication** via `UserManager<AppUser>` (ASP.NET Identity) with cookie-based auth
  - Login / logout
  - Forgot password / reset password
  - Change email
  - 6-digit verification code
- **Role-based authorization**
- **Email service** (SMTP)

## Database Migrations
Navigate to the solution root first:

**Add Migration:**
```powershell
dotnet ef migrations add Mig_1 `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
```

**Apply Migration:**
```powershell
dotnet ef database update `
  --project .\DataAccess\DataAccess.csproj `
  --startup-project .\API\API.csproj
```
