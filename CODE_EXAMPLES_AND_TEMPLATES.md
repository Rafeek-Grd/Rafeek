# Rafeek Project - Code Examples & Implementation Templates

**Purpose:** Practical code snippets and templates for common development tasks  
**Last Updated:** April 2026  

---

## Quick Reference Templates

### 1. Creating a New Feature (Step-by-Step)

#### Step 1: Define the Command
**File:** `Rafeek.Application/Handlers/MyFeatureHandlers/Commands/CreateMyEntity/CreateMyEntityCommand.cs`

```csharp
using MediatR;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity
{
    public class CreateMyEntityCommand : IRequest<Unit>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
```

#### Step 2: Create the Command Handler
**File:** `Rafeek.Application/Handlers/MyFeatureHandlers/Commands/CreateMyEntity/CreateMyEntityCommandHandler.cs`

```csharp
using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity
{
    public class CreateMyEntityCommandHandler : IRequestHandler<CreateMyEntityCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public CreateMyEntityCommandHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMyEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<MyEntity>(request);
            
            _ctx.MyEntityRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
```

#### Step 3: Create the Command Validator
**File:** `Rafeek.Application/Handlers/MyFeatureHandlers/Commands/CreateMyEntity/CreateMyEntityCommandValidator.cs`

```csharp
using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity
{
    public class CreateMyEntityCommandValidator : AbstractValidator<CreateMyEntityCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateMyEntityCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer["Name_Required"])
                .MaximumLength(200).WithMessage(_localizer["Name_MaxLength"]);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(_localizer["Description_Required"]);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(_localizer["StartDate_Required"])
                .GreaterThan(DateTime.UtcNow).WithMessage(_localizer["StartDate_MustBeFuture"]);
        }
    }
}
```

#### Step 4: Create the Domain Entity
**File:** `Rafeek.Domain/Entities/MyEntity.cs`

```csharp
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class MyEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
```

#### Step 5: Create the Repository Interface (if custom queries needed)
**File:** `Rafeek.Domain/Repositories/Interfaces/IMyEntityRepository.cs`

```csharp
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Domain.Repositories.Interfaces
{
    public interface IMyEntityRepository : IGenericRepository<MyEntity, Guid>
    {
        Task<MyEntity?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<MyEntity>> GetActiveAsync(CancellationToken cancellationToken);
    }
}
```

#### Step 6: Implement the Repository (if needed)
**File:** `Rafeek.Infrastructure/Repositories/Implementations/MyEntityRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class MyEntityRepository : GenericRepository<MyEntity, Guid>, IMyEntityRepository
    {
        public MyEntityRepository(IRafeekDbContext context) : base(context)
        {
        }

        public async Task<MyEntity?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await Table
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<MyEntity>> GetActiveAsync(CancellationToken cancellationToken)
        {
            return await Table
                .Where(x => x.IsActive)
                .ToListAsync(cancellationToken);
        }
    }
}
```

#### Step 7: Register Repository in UnitOfWork
**File:** `Rafeek.Infrastructure/Repositories/Implementations/Generic/UnitOfWork.cs`

```csharp
private IMyEntityRepository? _myEntityRepository;

public IMyEntityRepository MyEntityRepository 
    => _myEntityRepository ??= new MyEntityRepository(_context);
```

#### Step 8: Update the Interface
**File:** `Rafeek.Domain/Repositories/Interfaces/Generic/IUnitOfWork.cs`

```csharp
public interface IUnitOfWork : IAsyncDisposable
{
    IMyEntityRepository MyEntityRepository { get; }
    // ... other repositories
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

#### Step 9: Create the DTO
**File:** `Rafeek.Application/Common/Models/MyEntityDto.cs`

```csharp
namespace Rafeek.Application.Common.Models
{
    public class MyEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

#### Step 10: Create AutoMapper Profile
**File:** `Rafeek.Application/Mappings/MyEntityProfile.cs`

```csharp
using AutoMapper;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class MyEntityProfile : Profile
    {
        public MyEntityProfile()
        {
            CreateMap<CreateMyEntityCommand, MyEntity>();
            CreateMap<MyEntity, MyEntityDto>();
        }
    }
}
```

#### Step 11: Create the Controller Endpoint
**File:** `Rafeek.API/Controllers/Version1/MyEntityController.cs`

```csharp
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.MyEntities.Base)]
    public class MyEntityController : BaseApiController
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public MyEntityController(IMediator mediator, IStringLocalizer<Messages> localizer)
            : base(mediator, localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        [HttpPost(ApiRoutes.MyEntities.Create)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMyEntityCommand command)
        {
            var result = await Mediator.Send(command);
            return Created(ApiRoutes.MyEntities.GetById, result);
        }
    }
}
```

#### Step 12: Add Routes
**File:** `Rafeek.API/Routes/ApiRoutes.cs`

```csharp
public static class ApiRoutes
{
    public static class MyEntities
    {
        public const string Base = "api/v{version:apiVersion}/my-entities";
        public const string GetById = "{id}";
        public const string Create = "";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }
}
```

---

### 2. Creating a Query Handler

```csharp
using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Queries.GetMyEntityById
{
    public class GetMyEntityByIdQuery : IRequest<MyEntityDto>
    {
        public Guid Id { get; set; }
    }

    public class GetMyEntityByIdQueryHandler : IRequestHandler<GetMyEntityByIdQuery, MyEntityDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetMyEntityByIdQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<MyEntityDto> Handle(GetMyEntityByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.MyEntityRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (entity == null)
                throw new NotFoundException(nameof(MyEntity), request.Id);

            return _mapper.Map<MyEntityDto>(entity);
        }
    }
}
```

---

### 3. Creating an Update Handler

```csharp
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.UpdateMyEntity
{
    public class UpdateMyEntityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UpdateMyEntityCommandHandler : IRequestHandler<UpdateMyEntityCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public UpdateMyEntityCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateMyEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.MyEntities
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return false;

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
```

---

### 4. Creating a Delete Handler

```csharp
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.DeleteMyEntity
{
    public class DeleteMyEntityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteMyEntityCommandHandler : IRequestHandler<DeleteMyEntityCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public DeleteMyEntityCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteMyEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.MyEntities
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return false;

            _context.MyEntities.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
```

---

### 5. Complex Query with Pagination

```csharp
using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Queries.GetAllMyEntitiesPaginated
{
    public class GetAllMyEntitiesPaginatedQuery : IRequest<PaginatedResponse<MyEntityDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
    }

    public class GetAllMyEntitiesPaginatedQueryHandler 
        : IRequestHandler<GetAllMyEntitiesPaginatedQuery, PaginatedResponse<MyEntityDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetAllMyEntitiesPaginatedQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<MyEntityDto>> Handle(
            GetAllMyEntitiesPaginatedQuery request, 
            CancellationToken cancellationToken)
        {
            var query = _ctx.MyEntityRepository.GetQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Name.Contains(request.SearchTerm));

            if (request.IsActive.HasValue)
                query = query.Where(x => x.IsActive == request.IsActive.Value);

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var entities = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<MyEntityDto>>(entities);

            return new PaginatedResponse<MyEntityDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
```

---

### 6. Custom Validation Rule

```csharp
using FluentValidation;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity;

namespace Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity
{
    public class CreateMyEntityCommandValidator : AbstractValidator<CreateMyEntityCommand>
    {
        private readonly IUnitOfWork _ctx;

        public CreateMyEntityCommandValidator(IUnitOfWork ctx)
        {
            _ctx = ctx;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MustAsync(BeUniqueName).WithMessage("Name already exists");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            var exists = await _ctx.MyEntityRepository.GetByNameAsync(name, cancellationToken);
            return exists == null;
        }
    }
}
```

---

### 7. Custom Exception Usage

```csharp
using Rafeek.Application.Common.Exceptions;

public async Task<Unit> Handle(CreateMyEntityCommand request, CancellationToken cancellationToken)
{
    // Not found
    var existingEntity = await _ctx.MyEntityRepository.GetByIdAsync(request.Id, cancellationToken);
    if (existingEntity == null)
        throw new NotFoundException(nameof(MyEntity), request.Id);

    // Bad request
    if (existingEntity.IsLocked)
        throw new BadRequestException("This entity is locked and cannot be modified");

    // Unauthorized
    if (!user.IsAdmin)
        throw new UnauthorizedException("Admin role required to perform this action");

    // Validation exception
    if (request.StartDate > request.EndDate)
        throw new ValidationException("Start date must be before end date");

    // ... rest of handler
}
```

---

### 8. Authorization in Controller

```csharp
[HttpPost(ApiRoutes.MyEntities.Create)]
[Authorize] // Requires authentication
public async Task<IActionResult> Create([FromBody] CreateMyEntityCommand command)
{
    // Any authenticated user
}

[HttpDelete(ApiRoutes.MyEntities.Delete)]
[Authorize(Roles = "Admin")] // Requires Admin role
public async Task<IActionResult> Delete(Guid id)
{
    // Admin only
}

[HttpGet(ApiRoutes.MyEntities.GetById)]
[AllowAnonymous] // Public endpoint
public async Task<IActionResult> GetById(Guid id)
{
    // No authentication required
}

[HttpPut(ApiRoutes.MyEntities.Update)]
[Authorize(Policy = "CanEditEntities")] // Custom policy
public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMyEntityCommand command)
{
    // Users with CanEditEntities policy
}
```

---

### 9. Email Notification Handler

```csharp
using MediatR;
using Rafeek.Infrastructure.Notifications.Emails;

public class NotifyUserCommandHandler : IRequestHandler<NotifyUserCommand, Unit>
{
    private readonly IEmailService _emailService;

    public NotifyUserCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Unit> Handle(NotifyUserCommand request, CancellationToken cancellationToken)
    {
        var subject = "Important Notification";
        var body = $@"
            <h1>Hello {request.UserName},</h1>
            <p>Your application has been processed.</p>
            <p>Status: {request.Status}</p>
        ";

        await _emailService.SendEmailAsync(request.UserEmail, subject, body);
        return Unit.Value;
    }
}
```

---

### 10. File Upload Handler

```csharp
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Exceptions;

public class UploadStudentImageCommandHandler : IRequestHandler<UploadStudentImageCommand, string>
{
    private readonly IImageValidator _imageValidator;
    private readonly IUnitOfWork _ctx;

    public UploadStudentImageCommandHandler(IImageValidator imageValidator, IUnitOfWork ctx)
    {
        _imageValidator = imageValidator;
        _ctx = ctx;
    }

    public async Task<string> Handle(UploadStudentImageCommand request, CancellationToken cancellationToken)
    {
        // Validate file
        if (!await _imageValidator.ValidateAsync(request.Image))
            throw new BadRequestException("Invalid image file format or size");

        // Save file
        var fileName = $"{Guid.NewGuid()}.jpg";
        var uploadPath = Path.Combine("wwwroot/uploads", fileName);
        
        using (var stream = new FileStream(uploadPath, FileMode.Create))
        {
            await request.Image.CopyToAsync(stream, cancellationToken);
        }

        // Update database
        var student = await _ctx.StudentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
            throw new NotFoundException(nameof(Student), request.StudentId);

        student.ProfileImageUrl = $"/uploads/{fileName}";
        await _ctx.SaveChangesAsync(cancellationToken);

        return student.ProfileImageUrl;
    }
}
```

---

### 11. Batch Operation Handler

```csharp
using MediatR;
using Rafeek.Application.Common.Interfaces;

public class BulkEnrollStudentsCommandHandler : IRequestHandler<BulkEnrollStudentsCommand, BulkOperationResult>
{
    private readonly IUnitOfWork _ctx;

    public BulkEnrollStudentsCommandHandler(IUnitOfWork ctx)
    {
        _ctx = ctx;
    }

    public async Task<BulkOperationResult> Handle(BulkEnrollStudentsCommand request, CancellationToken cancellationToken)
    {
        var result = new BulkOperationResult();

        // Use transaction for atomic operation
        using (var transaction = await _ctx.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                foreach (var enrollment in request.Enrollments)
                {
                    var student = await _ctx.StudentRepository
                        .GetByIdAsync(enrollment.StudentId, cancellationToken);
                    
                    if (student == null)
                    {
                        result.Failed.Add(new { enrollment.StudentId, Error = "Student not found" });
                        continue;
                    }

                    var section = await _ctx.SectionRepository
                        .GetByIdAsync(enrollment.SectionId, cancellationToken);
                    
                    if (section == null)
                    {
                        result.Failed.Add(new { enrollment.SectionId, Error = "Section not found" });
                        continue;
                    }

                    var enrollmentEntity = new Enrollment
                    {
                        StudentId = student.Id,
                        SectionId = section.Id,
                        EnrolledDate = DateTime.UtcNow
                    };

                    _ctx.EnrollmentRepository.Add(enrollmentEntity);
                    result.Succeeded++;
                }

                await _ctx.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        return result;
    }
}
```

---

### 12. Entity Configuration Example

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

public class AcademicTermConfiguration : IEntityTypeConfiguration<AcademicTerm>
{
    public void Configure(EntityTypeBuilder<AcademicTerm> builder)
    {
        builder.ToTable(nameof(AcademicTerm));

        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.TermType)
            .HasConversion<string>()
            .HasMaxLength(50);

        // Relationships
        builder.HasOne(x => x.AcademicYear)
            .WithMany(x => x.Terms)
            .HasForeignKey(x => x.AcademicYearId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.CalendarEvents)
            .WithOne(x => x.AcademicTerm)
            .HasForeignKey(x => x.AcademicTermId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.AcademicYearId)
            .HasName("IX_AcademicTerms_AcademicYearId");

        builder.HasIndex(x => new { x.Name, x.AcademicYearId })
            .IsUnique()
            .HasName("IX_AcademicTerms_Name_AcademicYearId_Unique");
    }
}
```

---

### 13. MediatR Behavior for Logging

```csharp
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Rafeek.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var response = await next();

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "Long Running Request: {Name} ({ElapsedMilliseconds}ms)",
                    typeof(TRequest).Name,
                    stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
```

---

### 14. Testing Template

```csharp
using Xunit;
using Moq;
using Rafeek.Application.Handlers.MyFeatureHandlers.Commands.CreateMyEntity;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using AutoMapper;

public class CreateMyEntityCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateMyEntityCommandHandler _handler;

    public CreateMyEntityCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateMyEntityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesEntityAndSaves()
    {
        // Arrange
        var command = new CreateMyEntityCommand 
        { 
            Name = "Test Entity",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(1)
        };

        var entity = new MyEntity 
        { 
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            StartDate = command.StartDate
        };

        _mockMapper
            .Setup(m => m.Map<MyEntity>(command))
            .Returns(entity);

        var mockRepository = new Mock<IGenericRepository<MyEntity, Guid>>();
        _mockUnitOfWork
            .Setup(u => u.MyEntityRepository)
            .Returns(mockRepository.Object);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        mockRepository.Verify(r => r.Add(It.IsAny<MyEntity>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ThrowsException()
    {
        // Arrange
        var command = new CreateMyEntityCommand 
        { 
            Name = "",
            Description = "Test"
        };

        // Act & Assert
        // This will be caught by ValidationBehaviour before handler is called
    }
}
```

---

### 15. Database Migration Template

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rafeek.Persistence.Migrations
{
    public partial class AddMyEntityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyEntity_Name",
                table: "MyEntity",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "MyEntity");
        }
    }
}
```

---

### 16. Localization Resource File

**File:** `Rafeek.Application/Localization/Messages.en.resx`

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <xsd:schema>
    <!-- Schema definition -->
  </xsd:schema>
  
  <resheader name="resmimetype">
    <value>text/microsoft-resx</value>
  </resheader>

  <!-- Validation Messages -->
  <data name="Name_Required" xml:space="preserve">
    <value>Name is required</value>
  </data>
  <data name="Name_MaxLength" xml:space="preserve">
    <value>Name cannot exceed 200 characters</value>
  </data>
  
  <!-- Action Messages -->
  <data name="ActionResult_Created" xml:space="preserve">
    <value>Entity created successfully</value>
  </data>
  <data name="ActionResult_Updated" xml:space="preserve">
    <value>Entity updated successfully</value>
  </data>
  
  <!-- Error Messages -->
  <data name="NotFound" xml:space="preserve">
    <value>The requested resource was not found</value>
  </data>
  <data name="Unauthorized" xml:space="preserve">
    <value>You are not authorized to perform this action</value>
  </data>
</root>
```

---

### 17. SwaggerUI Configuration

```csharp
// Program.cs
builder.Services.AddSwaggerGen(options =>
{
    // ... existing config ...

    // Add JWT Bearer authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme",
        In = ParameterLocation.Header
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
```

---

### 18. Health Check Custom Implementation

```csharp
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Rafeek.Application.Common.Interfaces;

public class CustomHealthCheck : IHealthCheck
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomHealthCheck(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check database connectivity
            var entityCount = await _unitOfWork.AcademicTermRepository.CountAsync(cancellationToken);
            
            var data = new Dictionary<string, object>
            {
                { "EntityCount", entityCount },
                { "Timestamp", DateTime.UtcNow }
            };

            return HealthCheckResult.Healthy("Database is operational", data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Database check failed: {ex.Message}");
        }
    }
}
```

---

### 19. Localization in Validators - Best Practice

```csharp
public class CreateAcademicTermCommandValidator : AbstractValidator<CreateAcademicTermCommand>
{
    private readonly IStringLocalizer<Messages> _localizer;

    public CreateAcademicTermCommandValidator(IStringLocalizer<Messages> localizer)
    {
        _localizer = localizer;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_localizer[LocalizationKeys.ValidationMessages.Required.Value, nameof(CreateAcademicTermCommand.Name)])
            .MaximumLength(200)
            .WithMessage(_localizer[LocalizationKeys.ValidationMessages.MaxLength.Value, 200]);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage(_localizer["EndDate_GreaterThanStartDate"]);
    }
}
```

---

## Common Code Snippets

### Get Current User ID
```csharp
var currentUserId = _currentUserService.UserId;
```

### Throw NotFoundException
```csharp
var entity = await repository.GetByIdAsync(id, cancellationToken);
if (entity == null)
    throw new NotFoundException(nameof(MyEntity), id);
```

### Pagination Helper
```csharp
var skip = (pageNumber - 1) * pageSize;
var entities = await query
    .Skip(skip)
    .Take(pageSize)
    .ToListAsync(cancellationToken);
```

### Include Related Entities
```csharp
var entity = await _context.AcademicTerms
    .Include(t => t.AcademicYear)
    .Include(t => t.CalendarEvents)
    .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
```

### Soft Delete Support
```csharp
// Add to entity configuration
builder.HasQueryFilter(x => !x.IsDeleted);

// In delete handler
entity.IsDeleted = true;
entity.DeletedAt = DateTime.UtcNow;
```

### Async Action in Controller
```csharp
[HttpGet(ApiRoutes.MyEntities.GetById)]
public async Task<IActionResult> GetById(Guid id)
{
    try
    {
        var query = new GetMyEntityByIdQuery { Id = id };
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
```

---

**Document Version:** 1.0  
**Status:** Ready for use  
**Last Updated:** April 2026
