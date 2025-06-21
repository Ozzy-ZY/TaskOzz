# TaskOzz - Task Management API

> **⚠️ PROJECT STATUS: DISCONTINUED**  
> This project has been discontinued and is no longer under active development. It serves as a learning resource and reference implementation for ASP.NET Core Web API development.

## 📋 Project Overview

TaskOzz is a RESTful API for managing personal tasks with user authentication, task CRUD operations, filtering, and pagination capabilities. Built using Clean Architecture principles with ASP.NET Core 9.0.

## 🏗️ Architectural Design

### Clean Architecture Implementation

This project follows Clean Architecture principles with a clear separation of concerns across four main layers:

```
TaskOzz/
├── DOMAIN/           # Core business logic and entities
├── APPLICATION/      # Application services and use cases
├── INFRASTRUCTURE/   # External concerns (database, external services)
└── API/             # Presentation layer (controllers, filters)
```

#### Layer Responsibilities

**DOMAIN Layer**
- Contains core business entities (`AppUser`, `Task`, `RefreshToken`)
- Defines business rules and domain logic
- No dependencies on external frameworks
- Pure C# classes with business logic

**APPLICATION Layer**
- Orchestrates use cases and business workflows
- Contains DTOs, validators, and service interfaces
- Implements application-specific business rules
- Depends only on the DOMAIN layer

**INFRASTRUCTURE Layer**
- Handles data persistence with Entity Framework Core
- Manages database context and migrations
- Implements external service integrations
- Depends on DOMAIN and APPLICATION layers

**API Layer**
- Exposes REST endpoints
- Handles HTTP requests/responses
- Implements authentication and authorization
- Contains filters and middleware
- Depends on all other layers

### Design Patterns Used

- **Repository Pattern**: Abstracted data access through Entity Framework
- **DTO Pattern**: Data transfer objects for API communication
- **Validator Pattern**: FluentValidation for input validation
- **Result Pattern**: Consistent error handling and responses
- **Dependency Injection**: IoC container for service management

## 🛠️ Technology Stack

### Core Framework
- **ASP.NET Core 9.0** - Web API framework
- **.NET 9.0** - Runtime and development platform

### Database & ORM
- **Entity Framework Core 9.0** - Object-relational mapping
- **SQL Server** - Primary database
- **Microsoft.EntityFrameworkCore.SqlServer** - SQL Server provider

### Authentication & Security
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT middleware
- **Microsoft.AspNetCore.Identity** - Password hashing and user management
- **Refresh Tokens** - Token refresh mechanism

### Validation & Documentation
- **FluentValidation** - Input validation framework
- **Swagger/OpenAPI** - API documentation
- **Swashbuckle.AspNetCore** - Swagger integration
- **Scalar** - Modern API documentation UI

### Development Tools
- **JetBrains Rider** - Primary IDE
- **Entity Framework Tools** - Database migrations
- **Git** - Version control

## 📁 Project Structure

```
TaskOzz/
├── DOMAIN/
│   ├── Models/
│   │   ├── AppUser.cs              # User entity
│   │   ├── Task.cs                 # Task entity
│   │   ├── RefreshToken.cs         # Refresh token entity
│   │   ├── AuthResult.cs           # Authentication result
│   │   └── ModelConfigs/           # Entity configurations
│   └── DOMAIN.csproj
├── APPLICATION/
│   ├── Services/
│   │   ├── Auth/
│   │   │   ├── AuthService.cs      # Authentication logic
│   │   │   └── JwtService.cs       # JWT token management
│   │   ├── TaskService.cs          # Task business logic
│   │   ├── PasswordService.cs      # Password hashing
│   │   ├── ImageService.cs         # File upload handling
│   │   ├── StatusFlags.cs          # Status codes
│   │   └── Helpers/                # Helper classes
│   ├── DTOs/                       # Data transfer objects
│   ├── Validator/                  # FluentValidation rules
│   ├── DependencyInjection.cs      # Service registration
│   └── APPLICATION.csproj
├── INFRASTRUCTURE/
│   ├── DataAccess/
│   │   ├── AppDbContext.cs         # Entity Framework context
│   │   └── AppDbContextFactory.cs  # Context factory
│   ├── Migrations/                 # Database migrations
│   ├── DependencyInjection.cs      # Infrastructure services
│   └── INFRASTRUCTURE.csproj
├── API/
│   ├── Controllers/
│   │   ├── AuthController.cs       # Authentication endpoints
│   │   ├── TaskController.cs       # Task management endpoints
│   │   └── ImageController.cs      # File upload endpoints
│   ├── Filters/
│   │   └── ValidationFilter.cs     # Global validation filter
│   ├── Program.cs                  # Application entry point
│   ├── appsettings.json            # Configuration
│   ├── Reqs.md                     # Requirements documentation
│   └── API.csproj
├── TaskOzz.sln                     # Solution file
└── README.md                       # This file
```

## 🔐 Security Features

### Authentication
- JWT-based authentication with configurable expiration
- Refresh token mechanism for extended sessions
- Password hashing using ASP.NET Core Identity
- Role-based authorization (User/Admin roles)

### Input Validation
- FluentValidation for comprehensive input validation
- Global validation filter for consistent error responses
- SQL injection prevention through Entity Framework
- XSS protection through proper input sanitization

### API Security
- CORS policy configuration
- HTTPS redirection
- JWT token validation
- Secure password requirements

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code or Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TaskOzz
   ```

2. **Configure database connection**
   - Update `API/appsettings.json` with your SQL Server connection string
   - Ensure the database server is running

3. **Run database migrations**
   ```bash
   cd INFRASTRUCTURE
   dotnet ef database update
   ```

4. **Configure JWT settings**
   - Update JWT secret key in `API/appsettings.json`
   - Set appropriate issuer and audience values

5. **Run the application**
   ```bash
   cd API
   dotnet run
   ```

6. **Access the API**
   - API: `https://localhost:7104`
   - Scalar Documentation: `https://localhost:7104/scalar`

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `GET /api/auth/me` - Get current user info
- `PUT /api/auth/changepassword` - Change password
- `POST /api/auth/refreshtoken` - Refresh access token
- `POST /api/auth/revoketoken` - Revoke refresh token

### Task Management Endpoints
- `GET /api/task` - List tasks with filtering and pagination
- `GET /api/task/{id}` - Get specific task
- `POST /api/task` - Create new task
- `PUT /api/task` - Update existing task
- `DELETE /api/task/{id}` - Delete task

### Query Parameters
- `filterKey` - Search in title and description
- `sortKey` - Sort by field (title, priority, status, etc.)
- `sortOrder` - Sort direction (asc/desc)
- `page` - Page number for pagination
- `pageSize` - Items per page

## 🔧 Configuration

### JWT Settings
```json
{
  "Jwt": {
    "ValidIssuer": "https://localhost:7104",
    "ValidAudience": "https://localhost:7104",
    "SecretKey": "your-secret-key-here",
    "ExpiryMinutes": 2,
    "RefreshTokenExpiryHours": 1
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YourServer;Initial Catalog=Taskozz;Integrated Security=True;..."
  }
}
```

## 🧪 Testing

### Manual Testing
- Use Swagger UI for interactive API testing
- Test authentication flow with register/login endpoints
- Verify task CRUD operations with proper authorization
- Test filtering and pagination functionality

### Recommended Testing Tools
- **Postman** - API testing and collection management
- **Swagger UI** - Built-in API documentation and testing
- **SQL Server Management Studio** - Database inspection

## 📝 Development Notes

### Key Design Decisions
1. **Clean Architecture**: Ensures maintainability and testability
2. **JWT Authentication**: Stateless authentication for scalability
3. **Entity Framework**: ORM for simplified data access
4. **FluentValidation**: Comprehensive input validation
5. **Result Pattern**: Consistent error handling across the application

### Known Limitations
- No unit tests implemented
- Limited error handling in some areas
- Basic CORS configuration
- No rate limiting
- No comprehensive logging

## 🚫 Project Discontinuation Notice

**This project has been discontinued and is no longer under active development.**

### Reasons for Discontinuation
- Project served its learning objectives
- Focus shifted to other priorities
- No longer aligned with current technology stack preferences

### What This Means
- No new features will be added
- No bug fixes will be implemented
- No security updates will be provided
- The codebase is provided as-is for reference purposes

### For Future Reference
This project demonstrates:
- Clean Architecture implementation in ASP.NET Core
- JWT authentication setup
- Entity Framework Core usage
- RESTful API design patterns
- Input validation and error handling

## 📄 License

This project is provided as-is for educational and reference purposes. No warranty is provided.

## 👥 Contributing

This project is discontinued and not accepting contributions. However, the codebase is available for learning and reference purposes.

---

**Last Updated**: June 2025  
**Project Status**: Discontinued  
**Framework Version**: ASP.NET Core 9.0 