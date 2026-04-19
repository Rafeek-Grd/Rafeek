# Rafeek Project - Comprehensive Coding Rules & Guidelines

**Project Type:** Enterprise Academic Management System  
**Architecture:** Clean Architecture with Vertical Slice Organization  
**Technology Stack:** ASP.NET Core 8+, Entity Framework Core, MediatR, AutoMapper, FluentValidation  
**Last Updated:** April 2026  

---

## Table of Contents
1. [Project Overview](#project-overview)
2. [Architecture & Layer Structure](#architecture--layer-structure)
3. [Naming Conventions](#naming-conventions)
4. [Code Organization Patterns](#code-organization-patterns)
5. [Handler Implementation Rules](#handler-implementation-rules)
6. [Entity & Data Model Rules](#entity--data-model-rules)
7. [Validation Rules](#validation-rules)
8. [API Controller Rules](#api-controller-rules)
9. [Dependency Injection Rules](#dependency-injection-rules)
10. [Database & Repository Rules](#database--repository-rules)
11. [Error Handling & Exceptions](#error-handling--exceptions)
12. [Localization & Internationalization](#localization--internationalization)
13. [File Upload Rules](#file-upload-rules)
14. [API Versioning](#api-versioning)
15. [Logging & Monitoring](#logging--monitoring)
16. [Security Rules](#security-rules)
17. [Best Practices](#best-practices)

---

## Project Overview

**Rafeek** is an enterprise academic management system designed to streamline academic operations including:
- Academic calendar and term management
- Student enrollment and academic profiles
- Advisor-student relationships and guidance
- Course management and prerequisites
- Grade management and GPA calculations
- File uploads and document management
- Real-time notifications and communication
- Authentication and authorization with JWT tokens

### Core Features
- Multi-language support (Localization)
- Rate limiting for API protection
- Health checks and monitoring
- File upload handling with tusdotnet
- Swagger/OpenAPI documentation
- Role-based access control (RBAC)
- API versioning support

---

## Architecture & Layer Structure

### Solution Structure
```
Rafeek.sln
├── Rafeek.API (Presentation Layer)
├── Rafeek.Application (Application Layer)
├── Rafeek.Domain (Domain Layer)
├── Rafeek.Infrastructure (Infrastructure Layer)
└── Rafeek.Persistence (Data Access Layer)
```

### Layer Responsibilities

#### 1. **Rafeek.Domain**
- **Purpose:** Contains core business entities and domain logic
- **Contents:** Entities, Enums, Repository Interfaces, Domain Models
- **Rules:**
  - No dependencies on external frameworks except for base types
  - All entities must inherit from `BaseEntity`
  - Contains domain-specific enums and value objects
  - Never reference any other layer

#### 2. **Rafeek.Persistence**
- **Purpose:** Implements data access and database context
- **Contents:** DbContext, Entity Configurations, Migrations, Seed Data
- **Rules:**
  - Entity configuration via Fluent API in `OnModelCreating()`
  - `RafeekDbContext` for business entities
  - `RafeekIdentityDbContext` for identity entities
  - Only depends on Domain and Infrastructure

#### 3. **Rafeek.Infrastructure**
- **Purpose:** Implements external services and infrastructure concerns
- **Contents:** Repositories, Identity, JWT, Notifications, OAuth, Data Protection
- **Rules:**
  - Implements interfaces defined in Domain
  - Handles JWT token generation and refresh tokens
  - Email notification services
  - Identity management and user repositories
  - Data protection services

#### 4. **Rafeek.Application**
- **Purpose:** Contains business logic and orchestration (Command/Query handlers)
- **Contents:** MediatR Handlers, DTOs, Mappings, Behaviors, Localization, Validation
- **Rules:**
  - Implements use cases via MediatR handlers
  - FluentValidation validators for all commands
  - AutoMapper profiles for entity-to-DTO mapping
  - Pipeline behaviors for cross-cutting concerns
  - Health checks implementation

#### 5. **Rafeek.API**
- **Purpose:** HTTP API endpoints and presentation logic
- **Contents:** Controllers, Filters, Routes, Options, Swagger Configuration
- **Rules:**
  - RESTful endpoints following OpenAPI standards
  - Controllers inherit from `BaseApiController`
  - Version 1 endpoints under `Controllers/Version1/`
  - API configuration in `Program.cs`
  - Global exception filters and authorization

---

## Naming Conventions

### General Rules
- **Language:** English only
- **Casing:** PascalCase for public identifiers, camelCase for private/local
- **Prefixes/Suffixes:** Use meaningful suffixes for different types

### Specific Naming Rules

| Entity Type | Suffix/Pattern | Example |
|---|---|---|
| Command Handlers | `Command` + `Handler` | `CreateAcademicTermCommand`, `CreateAcademicTermCommandHandler` |
| Query Handlers | `Query` + `Handler` | `GetStudentByIdQuery`, `GetStudentByIdQueryHandler` |
| Validators | `Validator` | `CreateAcademicTermCommandValidator` |
| DTOs/Models | `Dto`, `Vm`, `Model` | `AcademicTermDto`, `StudentViewModel` |
| Repositories | `Repository` | `AcademicTermRepository` |
| Services | `Service` | `CurrentUserService`, `HealthCheckResponseWriter` |
| Interfaces | `I` prefix | `IUnitOfWork`, `IAcademicTermRepository` |
| Enums | Singular or plural as needed | `TermType`, `RolePermissions` |
| Controllers | `Controller` suffix | `AcademicTermController` |
| Filters | `Filter` or `Attribute` | `ApiExceptionFilterAttribute`, `AuthorizeCheckOperationFilter` |

### Folder Organization
- **Handlers:** Grouped by entity → Feature → Command/Query
  ```
  Handlers/
  ├── AcademicTermHandlers/
  │   ├── Commands/
  │   │   └── CreateAcademicTerm/
  │   │       ├── CreateAcademicTermCommand.cs
  │   │       ├── CreateAcademicTermCommandHandler.cs
  │   │       └── CreateAcademicTermCommandValidator.cs
  │   └── Queries/
  ```
- **Controllers:** Version-based organization
  ```
  Controllers/
  ├── Version1/
  │   └── [EntityName]Controller.cs
  ```

---

## Code Organization Patterns

### Vertical Slice Architecture
Each feature is organized vertically with all related components in one folder:

```
AcademicTermHandlers/
├── Commands/
│   ├── CreateAcademicTerm/
│   │   ├── CreateAcademicTermCommand.cs
│   │   ├── CreateAcademicTermCommandHandler.cs
│   │   └── CreateAcademicTermCommandValidator.cs
│   ├── UpdateAcademicTerm/
│   │   ├── UpdateAcademicTermCommand.cs
│   │   ├── UpdateAcademicTermCommandHandler.cs
│   │   └── UpdateAcademicTermCommandValidator.cs
│   └── DeleteAcademicTerm/
│       ├── DeleteAcademicTermCommand.cs
│       └── DeleteAcademicTermCommandHandler.cs
└── Queries/
    ├── GetAcademicTermById/
    ├── GetAllAcademicTerms/
    └── GetAcademicTermsByYear/
```

### File Organization Rules
- **One primary type per file** (with nested validators/handlers acceptable in same folder)
- **Descriptive file names** matching the class name
- **Consistent folder depth** within handler structure
- **Separate Commands and Queries** in different folders

---

## Handler Implementation Rules

### Command Handlers (Create/Update/Delete)

#### Rules:
1. **Interface Implementation:** `IRequestHandler<TCommand, TResponse>`
2. **Constructor Injection:** Accept `IUnitOfWork` and `IMapper` (typically)
3. **Return Type:** 
   - `Unit` for simple create/delete
   - `bool` for update/delete with status
   - DTO for operations returning data
4. **Async Implementation:** Must be async (`async Task<T>`)
5. **Error Handling:** Throw application exceptions, not catch globally
6. **Validation:** Handled by FluentValidation pipeline, not in handler

#### Template:
```csharp
public class CreateAcademicTermCommandHandler : IRequestHandler<CreateAcademicTermCommand, Unit>
{
    private readonly IUnitOfWork _ctx;
    private readonly IMapper _mapper;

    public CreateAcademicTermCommandHandler(IUnitOfWork ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateAcademicTermCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<AcademicTerm>(request);
        _ctx.AcademicTermRepository.Add(entity);
        await _ctx.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
```

### Query Handlers (Read-Only)

#### Rules:
1. **No State Changes:** Should only read from database
2. **Use Projection:** Map directly to DTOs when possible
3. **Lazy Loading:** Use `.Include()` explicitly
4. **Cancellation Token:** Always pass through
5. **Return Type:** DTOs, ViewModels, or Paged Results

#### Template:
```csharp
public class GetAcademicTermByIdQueryHandler : IRequestHandler<GetAcademicTermByIdQuery, AcademicTermDto>
{
    private readonly IUnitOfWork _ctx;
    private readonly IMapper _mapper;

    public GetAcademicTermByIdQueryHandler(IUnitOfWork ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public async Task<AcademicTermDto> Handle(GetAcademicTermByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _ctx.AcademicTermRepository
            .GetByIdAsync(request.Id, cancellationToken);
        
        if (entity == null)
            throw new NotFoundException(nameof(AcademicTerm), request.Id);

        return _mapper.Map<AcademicTermDto>(entity);
    }
}
```

### Handler Registration

- Auto-registered via `AddMediatR(Assembly.GetExecutingAssembly())` in DependencyInjection.cs
- Validators auto-registered via `RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly())`

---

## Entity & Data Model Rules

### BaseEntity Requirements
All entities must inherit from `BaseEntity`:

```csharp
public class AcademicTerm : BaseEntity
{
    // Core properties
    public string Name { get; set; } = null!;
    public TermType TermType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Relationships
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
    
    public ICollection<AcademicCalendar> CalendarEvents { get; set; } = new HashSet<AcademicCalendar>();
}
```

### Entity Rules
1. **ID Property:** Inherited from `BaseEntity` (typically `Guid`)
2. **Initialization:** Collections must be initialized with `new HashSet<T>()`
3. **Nullability:** Use `!= null!` for non-nullable reference types
4. **Navigation Properties:** 
   - Foreign key properties explicit: `public Guid EntityId { get; set; }`
   - Navigation property: `public Entity Entity { get; set; } = null!;`
5. **Timestamps:** Inherited timestamps from `BaseEntity` (CreatedAt, UpdatedAt, etc.)
6. **Soft Delete:** Support `IsDeleted` if needed

### Database Constraints
- Applied via Fluent API in `OnModelCreating()`
- Unique constraints for natural keys
- Foreign keys with cascade/restrict rules
- Indexes on frequently queried columns

### Entity Configuration Example
```csharp
public class AcademicTermConfiguration : IEntityTypeConfiguration<AcademicTerm>
{
    public void Configure(EntityTypeBuilder<AcademicTerm> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        
        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Terms)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

---

## Validation Rules

### FluentValidation Integration

#### Rules:
1. **One Validator per Command/Query:** Name matches `CommandName`Validator
2. **Inject Localizer:** Accept `IStringLocalizer<Messages>` for i18n messages
3. **Rule Chaining:** Use fluent API for readability
4. **Conditional Validation:** Use `.When()` for conditional rules
5. **Custom Validators:** Implement for complex business logic
6. **Error Messages:** Use localization keys or inject localizer

#### Template:
```csharp
public class CreateAcademicTermCommandValidator : AbstractValidator<CreateAcademicTermCommand>
{
    private readonly IStringLocalizer<Messages> _localizer;

    public CreateAcademicTermCommandValidator(IStringLocalizer<Messages> localizer)
    {
        _localizer = localizer;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End Date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be after Start Date.");

        RuleFor(x => x.AcademicYearId)
            .NotEmpty().WithMessage("Academic Year ID is required.");
    }
}
```

### Validation Pipeline Behavior
- Automatic validation via `ValidationBehaviour<TRequest, TResponse>` pipeline
- Throws `ValidationException` with errors on failure
- Errors serialized to client as `ApiResponse<T>` with validation details

---

## API Controller Rules

### Controller Structure

#### Rules:
1. **Inherit from BaseApiController**
2. **One Controller per Entity**
3. **Route Configuration:** Use `[Route(ApiRoutes.Base)]`
4. **Async Methods:** All actions must be async
5. **Return ApiResponse:** Use helper methods from BaseApiController
6. **Authorization:** Apply `[Authorize]`, `[AllowAnonymous]`, or role attributes

#### Template:
```csharp
[ApiVersion("1.0")]
[Route(ApiRoutes.AcademicTerms.Base)]
public class AcademicTermController : BaseApiController
{
    private readonly IStringLocalizer<Messages> _localizer;

    public AcademicTermController(
        IMediator mediator, 
        IStringLocalizer<Messages> localizer) 
        : base(mediator, localizer)
    {
        _localizer = localizer;
    }

    [HttpPost(ApiRoutes.AcademicTerms.Create)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAcademicTermCommand command)
    {
        var result = await Mediator.Send(command);
        return Created(ApiRoutes.AcademicTerms.GetById, result);
    }

    [HttpGet(ApiRoutes.AcademicTerms.GetById)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var query = new GetAcademicTermByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPut(ApiRoutes.AcademicTerms.Update)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAcademicTermCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return result ? Ok() : NotFound();
    }
}
```

### BaseApiController Helper Methods

```csharp
protected IActionResult Ok<TData>(TData? data, string message = null!)
protected IActionResult Created<TData>(string uri, TData data, string message = null!)
protected IActionResult Accepted<TData>(TData data, string message = null!)
protected IActionResult Deleted<TData>(TData data, string message = null!)
```

### Routes Configuration
Routes defined in `Routes/ApiRoutes.cs`:

```csharp
public static class ApiRoutes
{
    public static class AcademicTerms
    {
        public const string Base = "api/v{version:apiVersion}/academic-terms";
        public const string GetById = "{id}";
        public const string Create = "";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }
}
```

---

## Dependency Injection Rules

### Application Layer DI (`Rafeek.Application/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
{
    // AutoMapper
    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // FluentValidation
    services.AddFluentValidation(conf =>
    {
        conf.DisableDataAnnotationsValidation = true;
        conf.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

    // MediatR with Behaviors
    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

    return services;
}
```

### Infrastructure Layer DI (`Rafeek.Infrastructure/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    // Repositories
    services.AddScoped(typeof(IEntityRepository<,>), typeof(EntityRepository<,>));
    services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Identity & JWT
    services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => { /* config */ })
        .AddEntityFrameworkStores<RafeekIdentityDbContext>();
    
    services.AddTransient<IJwtTokenManager, JwtTokenManager>();

    return services;
}
```

### Persistence Layer DI (`Rafeek.Persistence/DependencyInjection.cs`)

```csharp
public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<RafeekDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddDbContext<RafeekIdentityDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

    services.AddScoped<IRafeekDbContext>(provider => provider.GetRequiredService<RafeekDbContext>());

    return services;
}
```

### Registration Pattern
- **Scoped:** Repositories, UnitOfWork, DbContext
- **Transient:** Handlers, Validators, Behaviors, Services
- **Singleton:** Configuration, Static Services

---

## Database & Repository Rules

### Unit of Work Pattern

All data operations go through `IUnitOfWork`:

```csharp
public interface IUnitOfWork : IAsyncDisposable
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IUserFbTokenRepository UserFbTokenRepository { get; }
    IAcademicCalendarRepository AcademicCalendarRepository { get; }
    IAcademicYearRepository AcademicYearRepository { get; }
    IAcademicTermRepository AcademicTermRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
```

### Repository Pattern Rules

1. **Generic Repository:** For standard CRUD operations
2. **Specific Repository:** For entity-specific complex queries
3. **Add, Update, Delete:** Via generic methods
4. **Save Changes:** Always through `UnitOfWork.SaveChangesAsync()`

#### Generic Repository Usage:
```csharp
var entity = new AcademicTerm { Name = "Fall 2025" };
_ctx.AcademicTermRepository.Add(entity);
await _ctx.SaveChangesAsync(cancellationToken);
```

#### Custom Repository (if needed):
```csharp
public interface IAcademicTermRepository : IGenericRepository<AcademicTerm, Guid>
{
    Task<IEnumerable<AcademicTerm>> GetByAcademicYearAsync(Guid yearId, CancellationToken cancellationToken);
}
```

### DbContext Configuration

#### RafeekDbContext
- Business entities (AcademicTerm, Student, Course, etc.)
- Handles audit fields (CreatedAt, UpdatedAt, CreatedBy, etc.)

#### RafeekIdentityDbContext
- Identity entities (Users, Roles, Claims, etc.)
- Separate context for security isolation

### Migrations

- **Location:** `Rafeek.Persistence/Migrations/`
- **Naming:** `YYYYMMDD_DescriptiveActionName`
- **Commands:**
  ```bash
  dotnet ef migrations add MigrationName --project Rafeek.Persistence --startup-project Rafeek.API
  dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API
  ```

---

## Error Handling & Exceptions

### Exception Hierarchy

```
ApplicationException (Base)
├── NotFoundException
├── UnauthorizedException
├── BadRequestException
├── ValidationException
└── InternalServerException
```

### Usage Rules

```csharp
// Not found
if (entity == null)
    throw new NotFoundException(nameof(AcademicTerm), id);

// Bad request
if (entity.IsLocked)
    throw new BadRequestException("Term is locked and cannot be updated");

// Validation
throw new ValidationException("Custom validation error");

// Unauthorized
if (!user.IsAdmin)
    throw new UnauthorizedException("Admin access required");
```

### Global Exception Filter

Applied via `ApiExceptionFilterAttribute` in Program.cs:

```csharp
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var exception = context.Exception;

        var response = exception switch
        {
            ApplicationException appEx => /* format response */,
            ValidationException valEx => /* format response */,
            _ => /* format response */
        };

        context.Result = new ObjectResult(response) { StatusCode = response.StatusCode };
        context.ExceptionHandled = true;
    }
}
```

---

## Localization & Internationalization

### Localization Files
- **Location:** `Rafeek.Application/Localization/`
- **Format:** Resource files (.resx) per language
- **Supported Languages:** English (en-US), Arabic (ar-SA), etc.

### LocalizationKeys Structure

```csharp
public static class LocalizationKeys
{
    public static class AcionResultMessage
    {
        public static readonly KeyString Created = new("ActionResult_Created");
        public static readonly KeyString Updated = new("ActionResult_Updated");
        public static readonly KeyString Deleted = new("ActionResult_Deleted");
        public static readonly KeyString Ok = new("ActionResult_Ok");
    }

    public static class ValidationMessages
    {
        public static readonly KeyString Required = new("Validation_Required");
        public static readonly KeyString InvalidFormat = new("Validation_InvalidFormat");
    }
}
```

### Usage in Code

```csharp
public class CreateAcademicTermCommandValidator : AbstractValidator<CreateAcademicTermCommand>
{
    private readonly IStringLocalizer<Messages> _localizer;

    public CreateAcademicTermCommandValidator(IStringLocalizer<Messages> localizer)
    {
        _localizer = localizer;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localizer["Name_Required"]);
    }
}
```

### Controller Usage

```csharp
protected IActionResult Ok<TData>(TData? data, string message = null!) 
    => base.Ok(ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Ok.Value]));
```

---

## File Upload Rules

### Configuration

```csharp
// Configured via tusdotnet in Program.cs
var tusStore = new TusDiskStore("/path/to/uploads");
var options = new DefaultTusConfiguration
{
    Store = tusStore,
    MetadataParserSelector = DefaultTusConfiguration.GetSimpleChunkingConfiguration(chunkSize: 5242880),
    UrlPath = "/files",
};

app.UseTus(httpContext => Task.FromResult((ITusConfiguration?)options));
```

### Validators

Specific validators for different file types:

```csharp
public interface IImageValidator { Task<bool> ValidateAsync(IFormFile file); }
public interface IVideoValidator { Task<bool> ValidateAsync(IFormFile file); }
public interface IAudioValidator { Task<bool> ValidateAsync(IFormFile file); }
public interface IFileValidator { Task<bool> ValidateAsync(IFormFile file); }
```

### Usage in Handlers

```csharp
public class UploadStudentImageCommandHandler : IRequestHandler<UploadStudentImageCommand, string>
{
    private readonly IImageValidator _imageValidator;

    public async Task<string> Handle(UploadStudentImageCommand request, CancellationToken cancellationToken)
    {
        if (!await _imageValidator.ValidateAsync(request.Image))
            throw new BadRequestException("Invalid image file");

        var fileName = $"{Guid.NewGuid()}.jpg";
        // Save file and return URI
    }
}
```

---

## API Versioning

### Configuration

```csharp
// Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
```

### Controller Implementation

```csharp
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/academic-terms")]
public class AcademicTermController : BaseApiController
{
    // Implementation
}
```

### URL Format
```
GET /api/v1/academic-terms
GET /api/v1.1/academic-terms
```

### Deprecation
- Mark deprecated endpoints with `[Obsolete]`
- Announce in API documentation and changelogs
- Support deprecated versions for minimum 1 major release cycle

---

## Logging & Monitoring

### Logging Framework
- **Library:** NLog
- **Configuration:** `nlog.config` at project root

### NLog Configuration Example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" 
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true" throwConfigExceptions="true" internalLogLevel="Off">

    <targets>
        <target name="file" xsi:type="File" fileName="logs/rafeek/${shortdate}.log" />
        <target name="console" xsi:type="ColoredConsole" layout="${date:format=HH\:mm\:ss}|${level:uppercase=true}|${logger}|${message}" />
    </targets>

    <rules>
        <logger name="*" minlevel="Info" writeTo="file,console" />
    </rules>
</nlog>
```

### Logging Usage

```csharp
private readonly ILogger<MyHandler> _logger;

public async Task<Unit> Handle(MyCommand request, CancellationToken cancellationToken)
{
    _logger.LogInformation("Processing command for entity {EntityId}", request.Id);
    
    try
    {
        // Process
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error processing command for entity {EntityId}", request.Id);
        throw;
    }
}
```

### Health Checks

- **Location:** `Rafeek.Application/HealthCheck/`
- **Endpoint:** `/health`
- **Implementation:**
  ```csharp
  public class ApplicationHealthCheck : IHealthCheck
  {
      public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
      {
          // Check database, cache, etc.
          return HealthCheckResult.Healthy("Application is healthy");
      }
  }
  ```

---

## Security Rules

### Authentication & Authorization

#### JWT Token Configuration
```csharp
var jwtOptions = new JwtOptions();
configuration.Bind(nameof(JwtOptions), jwtOptions);

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
        };
    });
```

#### Authorization Attributes
```csharp
[Authorize]                              // Authenticated users only
[Authorize(Roles = "Admin")]             // Specific role
[AllowAnonymous]                         // Public endpoint
[Authorize(Policy = "StudentOnly")]      // Custom policy
```

### Password Requirements
- Minimum length: 8 characters
- Require digits: True
- Require lowercase: True
- Require uppercase: True
- Require non-alphanumeric: True

### Data Protection
- **Encryption:** ASP.NET Core Data Protection API
- **Configuration:** `appsettings.json` / User Secrets
- **Connection Strings:** Never commit in code

### CORS Configuration
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://example.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### Rate Limiting
```csharp
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
```

### Refresh Token Rules
- **Storage:** Database with expiration
- **Rotation:** Generate new refresh token on use
- **Revocation:** Support manual revocation
- **Validation:** Check device fingerprint if applicable

---

## Best Practices

### Code Quality

1. **DRY Principle:** Don't Repeat Yourself
   - Extract common logic to utilities
   - Use base classes and interfaces
   - Avoid code duplication across handlers

2. **SOLID Principles**
   - **S**ingle Responsibility: One reason to change
   - **O**pen/Closed: Open for extension, closed for modification
   - **L**iskov Substitution: Derived classes must be substitutable
   - **I**nterface Segregation: Many specific interfaces vs. one general
   - **D**ependency Inversion: Depend on abstractions, not concrete types

3. **Consistency**
   - Follow naming conventions strictly
   - Consistent indentation (4 spaces)
   - Consistent null-coalescing patterns
   - Consistent error handling

### Performance Considerations

1. **Database Queries**
   - Use `.Include()` explicitly
   - Avoid N+1 queries
   - Use `.AsNoTracking()` for read-only queries
   - Implement pagination for large result sets

2. **Caching**
   - Cache read-heavy query results
   - Invalidate cache on writes
   - Use appropriate cache duration

3. **Async Operations**
   - Always use async for I/O operations
   - Never use `.Result` or `.Wait()`
   - Pass `CancellationToken` through chain

4. **Mapping**
   - Project at database level when possible
   - Use `.ProjectTo<Dto>()` in LINQ
   - Lazy-load related entities only when needed

### Testing Best Practices

1. **Unit Tests**
   - Test handlers with mocked repositories
   - Test validators independently
   - Aim for >80% code coverage

2. **Integration Tests**
   - Use in-memory database for speed
   - Test full request/response cycle
   - Test authorization/authentication

3. **Test Structure**
   ```csharp
   public class CreateAcademicTermCommandHandlerTests
   {
       [Fact]
       public async Task Handle_ValidCommand_CreatesEntity()
       {
           // Arrange
           // Act
           // Assert
       }
   }
   ```

### Documentation

1. **Code Comments**
   - Document complex business logic
   - Explain why, not what (code shows what)
   - Use XML comments for public APIs
   ```csharp
   /// <summary>
   /// Creates a new academic term for the specified academic year.
   /// </summary>
   /// <param name="command">The create command with term details</param>
   /// <param name="cancellationToken">Cancellation token</param>
   /// <returns>Unit representing successful completion</returns>
   public async Task<Unit> Handle(CreateAcademicTermCommand command, CancellationToken cancellationToken)
   ```

2. **API Documentation**
   - Swagger annotations on endpoints
   - Keep Swagger spec updated
   - Document request/response schemas

3. **README**
   - Project setup instructions
   - Running migrations
   - Configuration details

### Git Practices

1. **Commit Messages**
   ```
   feat: Add create academic term handler
   fix: Fix date validation in term command
   refactor: Restructure repository pattern
   docs: Update coding guidelines
   ```

2. **Branch Naming**
   ```
   feature/add-academic-terms
   bugfix/fix-date-validation
   hotfix/critical-security-issue
   ```

3. **Pull Requests**
   - Descriptive titles and descriptions
   - Reference related issues
   - Request reviews from team members
   - Ensure CI/CD passes

### Deployment Checklist

- [ ] All tests passing
- [ ] Code reviewed and approved
- [ ] Database migrations planned
- [ ] Configuration updated for environment
- [ ] Secrets configured in deployment pipeline
- [ ] Swagger documentation updated
- [ ] Performance impact assessed
- [ ] Security implications reviewed
- [ ] Rollback plan documented
- [ ] Deployment window scheduled

---

## Configuration & Environment Management

### Configuration Hierarchy

```
1. appsettings.json (Default)
2. appsettings.{Environment}.json (Environment-specific)
3. User Secrets (Development)
4. Environment Variables
5. Command-line Arguments
```

### Example appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Rafeek;Integrated Security=true;",
    "IdentityConnection": "Server=.;Database=RafeekIdentity;Integrated Security=true;"
  },
  "JwtOptions": {
    "Secret": "your-secret-key-min-32-characters",
    "Issuer": "rafeek-api",
    "Audience": "rafeek-client",
    "ExpirationMinutes": 15
  },
  "RefreshTokenOptions": {
    "ExpirationDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

### User Secrets (Development Only)
```bash
dotnet user-secrets init
dotnet user-secrets set "JwtOptions:Secret" "your-secret-here"
```

---

## Common Tasks & Solutions

### Creating a New Entity & Handler

1. **Create Entity** in `Rafeek.Domain/Entities/`
   ```csharp
   public class MyEntity : BaseEntity
   {
       public string Name { get; set; } = null!;
   }
   ```

2. **Create Repository Interface** in `Rafeek.Domain/Repositories/Interfaces/`
   ```csharp
   public interface IMyEntityRepository : IGenericRepository<MyEntity, Guid>
   {
   }
   ```

3. **Create Handler Structure** in `Rafeek.Application/Handlers/MyEntityHandlers/Commands/Create/`
   - `CreateMyEntityCommand.cs`
   - `CreateMyEntityCommandHandler.cs`
   - `CreateMyEntityCommandValidator.cs`

4. **Create Controller** in `Rafeek.API/Controllers/Version1/`
   ```csharp
   public class MyEntityController : BaseApiController
   {
   }
   ```

5. **Add Route** in `ApiRoutes.cs`
   ```csharp
   public static class MyEntities
   {
       public const string Base = "api/v{version:apiVersion}/my-entities";
       public const string Create = "";
   }
   ```

### Running Migrations

```bash
# Create migration
dotnet ef migrations add AddMyEntity --project Rafeek.Persistence --startup-project Rafeek.API

# Update database
dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API

# Revert migration
dotnet ef migrations remove --project Rafeek.Persistence --startup-project Rafeek.API
```

### Debugging & Troubleshooting

1. **Enable SQL Query Logging**
   ```csharp
   optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
   ```

2. **Check Validation Errors**
   - Validation errors returned in response
   - Check `CreateAcademicTermCommandValidator` for rules

3. **Token Issues**
   - Verify token not expired
   - Check JWT secret matches
   - Ensure token format: `Bearer {token}`

---

## Maintenance & Updates

### Regular Tasks
- **Weekly:** Review logs for errors
- **Monthly:** Review performance metrics
- **Quarterly:** Update dependencies
- **Annually:** Security audit and penetration testing

### Dependency Updates
```bash
dotnet package update --project Rafeek.API
dotnet package upgrade [package-name]
```

### Version Support Policy
- Current: Full support (bug fixes + features)
- N-1: Security fixes only
- N-2: Critical fixes only
- Older: No support

---

## Resources & References

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [AutoMapper Documentation](https://automapper.org/)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)

---

**Document Version:** 1.0  
**Last Updated:** April 2026  
**Maintained By:** Development Team  
**Status:** Active
