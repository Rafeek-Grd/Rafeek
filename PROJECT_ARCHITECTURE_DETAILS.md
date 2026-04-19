# Rafeek Project - Architecture & Detailed Technical Specifications

**Project:** Rafeek Academic Management System  
**Created:** April 2026  
**Tech Stack:** .NET 8, Clean Architecture, Vertical Slices  

---

## Table of Contents
1. [System Architecture Overview](#system-architecture-overview)
2. [Core Modules & Features](#core-modules--features)
3. [Technology Stack Details](#technology-stack-details)
4. [Entity Relationships](#entity-relationships)
5. [Request/Response Flow](#requestresponse-flow)
6. [Database Schema Overview](#database-schema-overview)
7. [Service Integration Points](#service-integration-points)
8. [Module-Specific Rules](#module-specific-rules)
9. [Performance Optimization Strategies](#performance-optimization-strategies)
10. [Deployment Architecture](#deployment-architecture)

---

## System Architecture Overview

### Layered Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    Rafeek.API (Presentation)                 │
│  Controllers | Filters | Routes | Swagger | Options         │
└─────────────────────────────────────────────────────────────┘
                              ↓ HTTP
┌─────────────────────────────────────────────────────────────┐
│               Rafeek.Application (Business Logic)            │
│ Handlers | Validators | DTOs | Mappings | Behaviors         │
│ (MediatR | FluentValidation | AutoMapper)                   │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│               Rafeek.Infrastructure (Services)               │
│ Repositories | JWT | Identity | Notifications | Oauth       │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│  Rafeek.Persistence (Data Access)                            │
│  DbContext | Migrations | Entity Configurations             │
└─────────────────────────────────────────────────────────────┘
                              ↓ EF Core
┌─────────────────────────────────────────────────────────────┐
│               SQL Server / Database                          │
└─────────────────────────────────────────────────────────────┘
                              ↑
                    Rafeek.Domain (Entities)
              (Entities | Enums | Repository Interfaces)
```

### Vertical Slice Organization

Each feature is organized vertically from controller → handler → entity:

```
Feature: Academic Term Management
├── API Layer
│   └── Controllers/Version1/AcademicTermController.cs
├── Application Layer
│   └── Handlers/AcademicTermHandlers/
│       ├── Commands/
│       │   ├── CreateAcademicTerm/
│       │   ├── UpdateAcademicTerm/
│       │   └── DeleteAcademicTerm/
│       └── Queries/
│           ├── GetAcademicTermById/
│           ├── GetAllAcademicTerms/
│           └── GetTermsByAcademicYear/
├── Domain Layer
│   └── Entities/AcademicTerm.cs
├── Infrastructure Layer
│   └── Repositories/AcademicTermRepository.cs
└── Persistence Layer
    ├── Configurations/AcademicTermConfiguration.cs
    └── DbContext (OnModelCreating)
```

---

## Core Modules & Features

### 1. Academic Calendar Management

**Purpose:** Manage academic calendars and important dates

**Entities:**
- `AcademicCalendar` - Calendar events with dates
- `AcademicYear` - Academic year container
- `AcademicTerm` - Term within academic year

**Handlers:**
- `AddEventToAcademicCalendarCommandHandler`
- `UpdateEventOfAcademicCalendarCommandHandler`
- `DeleteEventOfAcademicCalendarCommandHandler`
- `GetEventOfAcademicCalendarByIdQueryHandler`

**Key Features:**
- Multi-term support per academic year
- Registration, exam, and drop deadlines
- Event-based calendar items
- Date validation and constraints

### 2. Student Management

**Purpose:** Manage student records, enrollments, and academic profiles

**Core Entities:**
- `Student` - Student profile
- `StudentAcademicProfile` - GPA, status, progress
- `Enrollment` - Course enrollments
- `Grade` - Course grades

**Key Handlers:**
- `RequestGuidanceCommand` - Student requests guidance
- `AssignStudentToAcademicAdvisorCommand` - Assign advisor

**Key Features:**
- Student enrollment tracking
- GPA calculations
- Academic status management
- Guidance request system

### 3. Advisor & Guidance System

**Purpose:** Manage academic advisor relationships and guidance

**Entities:**
- `Doctor` / `Advisor` - Academic advisors
- `StudentSupport` - Support records

**Key Handlers:**
- `ReviewStudentGuidanceRequestCommand` - Advisors review requests

**Key Features:**
- One-to-many student-advisor relationships
- Guidance request workflow
- Support tracking

### 4. Authentication & Authorization

**Purpose:** Secure access control and user identity management

**Entities:**
- `ApplicationUser` - User identity (from ASP.NET Identity)
- `RefreshToken` - Token refresh mechanism

**Key Handlers:**
- `SignUpCommandHandler` - User registration
- `SignInCommandHandler` - User login
- `RefreshTokenCommandHandler` - Token refresh

**Key Features:**
- JWT token-based authentication
- Refresh token rotation
- Role-based access control (RBAC)
- Multi-language support for auth messages

### 5. File Upload & Management

**Purpose:** Handle file uploads for documents, images, and media

**Supported Types:**
- Images (JPG, PNG)
- Videos (MP4, AVI)
- Audio (MP3, WAV)
- General files (PDF, DOCX)

**Configuration:**
- tusdotnet for resumable uploads
- TUS protocol support
- Chunk-based upload (5MB chunks default)

**Validators:**
- `IImageValidator`
- `IVideoValidator`
- `IAudioValidator`
- `IFileValidator`

### 6. Notification System

**Purpose:** Send notifications to users

**Current Implementation:**
- Email notifications
- Extensible to SMS, Push notifications

**Key Services:**
- `IEmailService` - Email delivery
- `INotificationService` - Notification orchestration

### 7. GPA Simulator & Analytics

**Purpose:** Analyze academic performance and simulate scenarios

**Entities:**
- `GPASimulatorLog` - Simulation records
- `AnalyticsReport` - Performance analytics

**Features:**
- What-if GPA scenarios
- Academic performance reporting

---

## Technology Stack Details

### Core Framework
| Component | Version | Purpose |
|-----------|---------|---------|
| .NET | 8.0+ | Runtime environment |
| ASP.NET Core | 8.0+ | Web framework |
| Entity Framework Core | 8.0+ | ORM |
| SQL Server | 2019+ | Database |

### Libraries & Frameworks

| Library | Version | Purpose |
|---------|---------|---------|
| MediatR | Latest | Command/Query pattern |
| AutoMapper | Latest | Object mapping |
| FluentValidation | Latest | Input validation |
| AspNetCoreRateLimit | Latest | API rate limiting |
| NLog | Latest | Logging |
| Newtonsoft.Json | Latest | JSON serialization |
| Swashbuckle | Latest | Swagger/OpenAPI |
| tusdotnet | Latest | TUS protocol uploads |
| IdentityModel | Latest | JWT tokens |

### External Services
- **Email:** Configured via SMTP
- **OAuth:** Support for social login
- **Storage:** Local file system (configurable to cloud)

---

## Entity Relationships

### Simplified Entity Relationship Diagram

```
┌─────────────────────────┐
│   AcademicYear          │
├─────────────────────────┤
│ • Id (PK)               │
│ • Name                  │
│ • StartDate             │
│ • EndDate               │
└─────────────────────────┘
           │ 1:N
           ↓
┌─────────────────────────┐
│   AcademicTerm          │
├─────────────────────────┤
│ • Id (PK)               │
│ • Name                  │
│ • TermType              │
│ • StartDate             │
│ • EndDate               │
│ • AcademicYearId (FK)   │
└─────────────────────────┘
        │ 1:N
        ↓
┌─────────────────────────┐
│ AcademicCalendar        │
├─────────────────────────┤
│ • Id (PK)               │
│ • EventName             │
│ • EventDate             │
│ • AcademicTermId (FK)   │
└─────────────────────────┘


┌─────────────────────────┐
│   Course                │
├─────────────────────────┤
│ • Id (PK)               │
│ • Code                  │
│ • Name                  │
│ • Credits               │
│ • DepartmentId (FK)     │
└─────────────────────────┘
        │ 1:N             1:N (Prerequisites)
        ↓                 ↔
┌─────────────────────────┐  ┌──────────────────┐
│   Section               │  │ CoursePrerequisite│
├─────────────────────────┤  ├──────────────────┤
│ • Id (PK)               │  │ • CourseId (FK)  │
│ • SectionNumber         │  │ • PrereqId (FK)  │
│ • CourseId (FK)         │  └──────────────────┘
└─────────────────────────┘
        │ 1:N
        ↓
┌─────────────────────────┐
│   Enrollment            │
├─────────────────────────┤
│ • Id (PK)               │
│ • StudentId (FK)        │
│ • SectionId (FK)        │
│ • Grade (FK)            │
│ • Status                │
└─────────────────────────┘


┌──────────────────────────────┐
│     ApplicationUser (Identity)│
├──────────────────────────────┤
│ • Id (PK)                    │
│ • Email                      │
│ • PhoneNumber                │
│ • UserLoginHistory           │
└──────────────────────────────┘
     │ 1:N              1:N
     ↓                  ↓
┌────────────┐   ┌───────────────┐
│   Student  │   │ Doctor/Advisor│
├────────────┤   ├───────────────┤
│ • Id (PK)  │   │ • Id (PK)     │
│ • UserId   │   │ • UserId      │
│ • GPA      │   └───────────────┘
└────────────┘         │ 1:N
        │ 1:N          ↓
        ↓      ┌────────────────┐
┌──────────────┐│ StudentSupport │
│StudentProfile││                │
├──────────────┤│                │
│ • GPA        │└────────────────┘
│ • Status     │
└──────────────┘

┌─────────────────┐
│   RefreshToken  │
├─────────────────┤
│ • Id (PK)       │
│ • Token         │
│ • UserId (FK)   │
│ • ExpiresAt     │
│ • RevokedAt     │
└─────────────────┘
```

---

## Request/Response Flow

### Typical Request Flow

```
1. HTTP Request arrives at Controller
   │
   ├─→ URL Route matching
   ├─→ Model binding from body/query
   └─→ Authorization filters

2. Controller method executes
   │
   ├─→ Validate input via FluentValidation
   │   └─→ ValidationBehaviour intercepts via MediatR
   │
   ├─→ Send Command/Query via MediatR
   │   ├─→ UnhandledExceptionBehaviour (error handling)
   │   ├─→ ValidationBehaviour (validation)
   │   └─→ PerformanceBehaviour (logging)

3. Handler executes
   │
   ├─→ Map request to domain entity
   │   └─→ AutoMapper.Map<Entity>(request)
   │
   ├─→ Perform business logic
   │
   ├─→ Interact with repositories
   │   └─→ IUnitOfWork.EntityRepository
   │
   ├─→ Save changes
   │   └─→ await _ctx.SaveChangesAsync()

4. Return response to controller
   │
   ├─→ Map entity to response DTO
   │   └─→ AutoMapper.Map<ResponseDto>(entity)
   │
   └─→ Controller returns ApiResponse wrapper

5. HTTP Response sent to client
   │
   ├─→ JSON serialization (Newtonsoft.Json)
   ├─→ Appropriate HTTP status code
   └─→ ApiResponse<T> wrapper
```

### MediatR Pipeline Execution

```
Request
  ↓
┌─────────────────────────────────────────┐
│ UnhandledExceptionBehaviour             │
│ (wraps handler in try-catch)            │
├─────────────────────────────────────────┤
│ ValidationBehaviour                     │
│ (validates request before handler)      │
├─────────────────────────────────────────┤
│ PerformanceBehaviour                    │
│ (logs execution time)                   │
├─────────────────────────────────────────┤
│ Handler.Handle() - Executes             │
├─────────────────────────────────────────┤
│ PerformanceBehaviour (logs time)        │
│ ValidationBehaviour (passes through)    │
│ UnhandledExceptionBehaviour (passes)    │
└─────────────────────────────────────────┘
  ↓
Response
```

### API Response Structure

```csharp
{
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Fall 2025",
    "startDate": "2025-09-01T00:00:00",
    "endDate": "2025-12-15T00:00:00"
  },
  "message": "Academic term created successfully",
  "statusCode": 201,
  "succeeded": true,
  "errors": null
}
```

---

## Database Schema Overview

### Connection Strings Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVER_NAME;Database=Rafeek;User Id=SA;Password=PASSWORD;Encrypt=false;",
    "IdentityConnection": "Server=SERVER_NAME;Database=RafeekIdentity;User Id=SA;Password=PASSWORD;Encrypt=false;"
  }
}
```

### Database Contexts

#### RafeekDbContext
**Purpose:** Main business database for academic entities

**Key DbSets:**
- DbSet<AcademicYear>
- DbSet<AcademicTerm>
- DbSet<AcademicCalendar>
- DbSet<Student>
- DbSet<Course>
- DbSet<Section>
- DbSet<Enrollment>
- DbSet<Grade>
- And 20+ more entities

**Audit Features:**
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
- CreatedBy (UserId)
- UpdatedBy (UserId)

#### RafeekIdentityDbContext
**Purpose:** Identity and authentication data

**Key DbSets:**
- DbSet<ApplicationUser>
- DbSet<IdentityRole<Guid>>
- DbSet<IdentityUserClaim<Guid>>
- DbSet<IdentityUserRole<Guid>>
- DbSet<IdentityRoleClaim<Guid>>
- DbSet<RefreshToken>

### Key Tables

| Table | Purpose | Key Fields |
|-------|---------|-----------|
| AcademicYears | Academic year records | Id, Name, StartDate, EndDate |
| AcademicTerms | Term records | Id, Name, TermType, Dates |
| AcademicCalendars | Important dates | Id, EventName, EventDate |
| Courses | Course catalog | Id, Code, Name, Credits |
| Sections | Course sections | Id, SectionNumber, CourseId |
| Enrollments | Student enrollments | Id, StudentId, SectionId |
| Grades | Student grades | Id, EnrollmentId, Grade |
| Students | Student records | Id, UserId, GPA |
| ApplicationUsers | User accounts | Id, Email, PhoneNumber |
| RefreshTokens | Auth tokens | Id, Token, UserId, ExpiresAt |

---

## Service Integration Points

### Authentication Service

```csharp
public interface IJwtTokenManager
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);
    RefreshToken GenerateRefreshToken(ApplicationUser user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
```

**Usage:**
```csharp
var accessToken = _jwtTokenManager.GenerateAccessToken(user, roles);
var refreshToken = _jwtTokenManager.GenerateRefreshToken(user);
```

### Email Notification Service

```csharp
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailAsync(string[] to, string subject, string body);
}
```

**Usage:**
```csharp
await _emailService.SendEmailAsync(
    user.Email, 
    "Welcome to Rafeek", 
    "Your account has been created");
```

### Current User Service

```csharp
public interface ICurrentUserService
{
    string UserId { get; }
    string Email { get; }
    string UserName { get; }
    IEnumerable<string> Roles { get; }
}
```

**Usage:**
```csharp
var currentUserId = _currentUserService.UserId;
```

### File Validation Services

```csharp
public interface IImageValidator
{
    Task<bool> ValidateAsync(IFormFile file);
}

public interface IVideoValidator
{
    Task<bool> ValidateAsync(IFormFile file);
}

// Similar for Audio and general Files
```

---

## Module-Specific Rules

### Academic Calendar Module

**File Structure:**
```
AcademicCalendarHandlers/
├── Commands/
│   ├── AddEventToAcademicCalendar/
│   ├── UpdateEventOfAcademicCalendar/
│   └── DeleteEventOfAcademicCalendar/
└── Queries/
    ├── GetEventOfAcademicCalendarById/
    └── GetEventsByTerm/
```

**Validation Rules:**
- Event date must be within academic term
- Event date cannot be in the past
- Event name required (50-200 characters)
- No duplicate events on same date/term

**Key Business Logic:**
- Cascade delete events when term is deleted
- Prevent deletion of events with associated data
- Support recurring events (optional enhancement)

### Student Module

**File Structure:**
```
StudentHandlers/
├── Commands/
│   ├── RequestGuidanceCommand/
│   └── AssignStudentToAcademicAdvisorCommand/
└── Queries/
    ├── GetStudentByIdQuery/
    ├── GetStudentProfileQuery/
    └── GetStudentEnrollmentsQuery/
```

**Key Business Rules:**
- Students can enroll in prerequisites first
- Cannot exceed max credits per term
- GPA calculation via registered grades
- Academic standing based on GPA
- Guidance requests tracked with status

### Advisor Module

**File Structure:**
```
AdvisorHandlers/
├── Commands/
│   ├── ReviewStudentGuidanceRequestCommand/
│   ├── AssignStudentToAdvisorCommand/
│   └── UpdateAdvisorInfoCommand/
└── Queries/
    ├── GetAdvisedStudentsQuery/
    ├── GetGuidanceRequestsQuery/
```

**Key Business Rules:**
- Advisors can have multiple students
- Each student can have one primary advisor
- Guidance requests have status workflow
- Advisor notes are timestamped and auditable

### Authentication Module

**Security Considerations:**
- JWT tokens expire after 15 minutes
- Refresh tokens valid for 7 days
- Tokens include user roles and claims
- Refresh token rotation on use
- Support for token revocation
- Rate limiting on login attempts (5 per minute)

**Key Commands:**
- `SignUpCommand` - User registration with email
- `SignInCommand` - Authenticate user
- `RefreshTokenCommand` - Get new access token

---

## Performance Optimization Strategies

### Database Query Optimization

**1. Use Include() for Relationships**
```csharp
// Bad - N+1 query problem
var terms = await _ctx.AcademicTerms.ToListAsync();
foreach (var term in terms)
{
    var events = await _ctx.AcademicCalendars
        .Where(e => e.AcademicTermId == term.Id)
        .ToListAsync();
}

// Good - Single query with eager loading
var terms = await _ctx.AcademicTerms
    .Include(t => t.CalendarEvents)
    .ToListAsync();
```

**2. Use AsNoTracking() for Read-Only Queries**
```csharp
// Query that doesn't need tracking
var terms = await _ctx.AcademicTerms
    .AsNoTracking()
    .ToListAsync();
```

**3. Project to DTOs at Database Level**
```csharp
// Bad - Load entities then map
var terms = await _ctx.AcademicTerms.ToListAsync();
var dtos = _mapper.Map<List<AcademicTermDto>>(terms);

// Good - Project directly to DTO in query
var dtos = await _ctx.AcademicTerms
    .ProjectTo<AcademicTermDto>(_mapper.ConfigurationProvider)
    .ToListAsync();
```

### Caching Strategy

```csharp
// Cache academic years (rarely change)
public async Task<List<AcademicYearDto>> GetAllAcademicYears()
{
    var cacheKey = "academic_years_all";
    if (!_cache.TryGetValue(cacheKey, out var cached))
    {
        cached = await _ctx.AcademicYears
            .ProjectTo<AcademicYearDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        _cache.Set(cacheKey, cached, TimeSpan.FromHours(24));
    }
    return (List<AcademicYearDto>)cached;
}
```

### API Response Pagination

```csharp
// Implement pagination for large datasets
public class PaginatedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PaginatedResponse<T>
{
    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}

// Usage in query
var total = await _ctx.Students.CountAsync();
var students = await _ctx.Students
    .Skip((request.PageNumber - 1) * request.PageSize)
    .Take(request.PageSize)
    .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
    .ToListAsync();

return new PaginatedResponse<StudentDto>
{
    Data = students,
    TotalCount = total,
    PageNumber = request.PageNumber
};
```

### Indexing Strategy

Critical indexes to add to database:

```csharp
// In OnModelCreating
builder.Entity<AcademicTerm>()
    .HasIndex(t => t.AcademicYearId)
    .HasName("IX_AcademicTerms_AcademicYearId");

builder.Entity<Student>()
    .HasIndex(s => s.Email)
    .IsUnique()
    .HasName("IX_Students_Email_Unique");

builder.Entity<Enrollment>()
    .HasIndex(e => new { e.StudentId, e.SectionId })
    .HasName("IX_Enrollments_StudentId_SectionId");
```

---

## Deployment Architecture

### Development Environment
- **Machine:** Local development PC
- **Database:** LocalDB or SQL Server Express
- **Logging:** File-based (NLog to /logs)
- **Configuration:** appsettings.Development.json
- **Secrets:** User Secrets for sensitive data

### Staging Environment
- **Server:** Windows Server or Azure App Service
- **Database:** SQL Server 2019+
- **Configuration:** appsettings.Staging.json
- **SSL:** HTTPS enforced
- **Logging:** Centralized logging (ELK or similar)

### Production Environment
- **Deployment:** Docker containerized or Azure App Service
- **Database:** SQL Server Managed Instance or Azure SQL
- **Load Balancing:** Application Gateway or similar
- **Configuration:** Environment variables + Azure Key Vault
- **Monitoring:** Application Insights, Azure Monitor
- **Backup:** Automated daily backups with retention

### CI/CD Pipeline

```
┌─ GitHub Repository
│   └─ Push to main branch
│
├─ Build Step
│   ├─ Restore NuGet packages
│   ├─ Compile solution
│   └─ Run unit tests
│
├─ Quality Gates
│   ├─ Code coverage > 80%
│   ├─ No security vulnerabilities
│   └─ All tests passing
│
├─ Staging Deployment
│   ├─ Run migrations
│   ├─ Deploy to staging
│   └─ Run integration tests
│
└─ Production Deployment
    ├─ Manual approval
    ├─ Run migrations
    ├─ Blue-green deployment
    └─ Smoke tests
```

### Docker Configuration

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Rafeek.API/Rafeek.API.csproj", "Rafeek.API/"]
RUN dotnet restore "Rafeek.API/Rafeek.API.csproj"
COPY . .
RUN dotnet build "Rafeek.API/Rafeek.API.csproj" -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Rafeek.API.dll"]
```

### Database Migration Strategy

```bash
# Before deployment
dotnet ef migrations add [MigrationName] --project Rafeek.Persistence --startup-project Rafeek.API

# During deployment
dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API

# Rollback if needed
dotnet ef migrations remove --project Rafeek.Persistence --startup-project Rafeek.API
```

---

## Configuration by Environment

### Development
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*",
  "RateLimiting": false,
  "DetailedErrors": true
}
```

### Staging
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "staging.rafeek.com",
  "RateLimiting": true,
  "DetailedErrors": false
}
```

### Production
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error"
    }
  },
  "AllowedHosts": "rafeek.com",
  "RateLimiting": true,
  "DetailedErrors": false,
  "EnableHttps": true
}
```

---

## Key Metrics & Monitoring

### Application Metrics to Track
- **Response Time:** Target < 500ms for 95th percentile
- **Error Rate:** Target < 0.1%
- **Database Query Time:** Monitor slow queries
- **API Throttling:** Monitor rate limit violations
- **Authentication Failures:** Track suspicious activity

### Health Check Endpoints

```
GET /health - Overall application health
GET /health/detailed - Detailed health information
```

### Logging Guidelines

```csharp
// Log at different levels:
_logger.LogDebug("Debug info for development only");
_logger.LogInformation("Normal operation flow");
_logger.LogWarning("Something unusual happened");
_logger.LogError(ex, "An error occurred");
_logger.LogCritical(ex, "Critical system failure");
```

---

## Security Checklist for Production

- [ ] HTTPS/TLS enforced for all endpoints
- [ ] JWT secret stored in Key Vault (not in code)
- [ ] SQL injection prevented (ORM + parameterized queries)
- [ ] Cross-Site Scripting (XSS) prevention
- [ ] Cross-Site Request Forgery (CSRF) protection
- [ ] Authentication required for all protected endpoints
- [ ] Authorization rules properly implemented
- [ ] Sensitive data not logged
- [ ] Password requirements enforced
- [ ] Refresh tokens rotated
- [ ] Rate limiting enabled
- [ ] CORS configured restrictively
- [ ] Security headers set (X-Frame-Options, etc.)
- [ ] Dependency vulnerabilities scanned
- [ ] Regular security audits scheduled

---

## Troubleshooting Guide

### Common Issues & Solutions

| Issue | Cause | Solution |
|-------|-------|----------|
| 401 Unauthorized | Invalid/expired token | Check token expiry, regenerate token |
| 403 Forbidden | User lacks required role | Verify user role assignment |
| 500 Internal Server Error | Unhandled exception | Check logs, review stack trace |
| Slow queries | Missing indexes | Add indexes to FK columns |
| N+1 query problem | Missing Include() | Add .Include() to query |
| Validation error | Invalid input data | Check request payload, validator rules |
| Migration failed | Database schema issue | Review migration, check constraints |
| Token refresh fails | Refresh token expired | User must sign in again |

---

**Last Updated:** April 2026  
**Architecture Version:** 1.0  
**Status:** Active & Maintained
