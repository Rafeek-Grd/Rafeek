# FEATURE WORKFLOW — Rafeek .NET Backend

**How to use:** Copy this file into your AI IDE chat at the start of a new feature session.  
Replace all `{placeholders}` with your actual values before sending.  
The AI will follow these steps in order and **stop at every checkpoint** to wait for your approval.

---

## 🎯 FEATURE REQUEST

```
Feature Name  : {e.g., Manage Student Warnings}
Target Entity : {e.g., StudentWarning}
Operations    : {e.g., Create, Get by Id, Get all for student, Delete}
Business Rules: {e.g., A student can have at most 3 active warnings; warn advisor on 3rd}
Roles Required: {e.g., Admin can create; Advisor can read; Student can read own}
Relations     : {e.g., belongs to Student (FK: StudentId), created by Doctor (FK: DoctorId)}
```

---

## STEP 1 — Analysis & Design Review

**AI Task:**
1. Read and list all existing entities in `Rafeek.Domain/Entities/` that relate to `{Entity}`.
2. Check `Rafeek.Domain/Repositories/Interfaces/Generic/IUnitOfWork.cs` for existing repositories.
3. Check `Rafeek.API/Routes/ApiRoutes.cs` for existing route groups.
4. Check `Rafeek.Application/Localization/` for existing localization keys to reuse.
5. Identify if a custom repository interface is needed or if `IGenericRepository<T, Guid>` is sufficient.
6. Propose the final list of files to create/modify (no code yet).

**Output expected:**
- Bullet list: files to create (NEW) and files to modify (MODIFY)
- Proposed `{Entity}` properties and their types
- Proposed navigation properties and FK relationships
- Proposed API route paths

> ⛔ **CHECKPOINT 1** — Stop here. Wait for developer approval before writing any code.

---

## STEP 2 — Domain Layer

**AI Task:** Create the domain entity.

### 2.1 — [NEW] `Rafeek.Domain/Entities/{Entity}.cs`
- Inherits `BaseEntity`
- All required properties with correct nullability
- Navigation properties with `= null!` for required refs
- Collections initialized with `new HashSet<T>()`

> ⛔ **CHECKPOINT 2** — Stop. Wait for approval of the entity before proceeding.

---

## STEP 3 — Repository Layer (only if custom queries needed)

**AI Task:** Create the repository interface and implementation.

### 3.1 — [NEW or SKIP] `Rafeek.Domain/Repositories/Interfaces/I{Entity}Repository.cs`
- Extends `IGenericRepository<{Entity}, Guid>`
- Declare only methods that are NOT available in the generic repository
- Skip this file entirely if only standard CRUD is needed

### 3.2 — [NEW or SKIP] `Rafeek.Infrastructure/Repostiories/Implementations/{Entity}Repository.cs`
- Implements `I{Entity}Repository`
- Extends `GenericRepository<{Entity}, Guid>`
- Inject `IRafeekDbContext` via base constructor

### 3.3 — [MODIFY] `Rafeek.Domain/Repositories/Interfaces/Generic/IUnitOfWork.cs`
- Add: `I{Entity}Repository {Entity}Repository { get; }`

### 3.4 — [MODIFY] `Rafeek.Infrastructure/Repostiories/Implementations/Generic/UnitOfWork.cs`
- Add lazy-initialized backing field and property for `{Entity}Repository`

> ⛔ **CHECKPOINT 3** — Stop. Wait for approval of the repository layer.

---

## STEP 4 — Persistence Layer

**AI Task:** Configure the entity for EF Core.

### 4.1 — [NEW] `Rafeek.Persistence/Configurations/{Entity}Configuration.cs`
- Implements `IEntityTypeConfiguration<{Entity}>`
- Table name = `nameof({Entity})`
- Define: required fields, max lengths
- Define: all FK relationships with correct `OnDelete` behavior
- Define: indexes for every FK column
- Define: unique indexes for natural keys if applicable

### 4.2 — [MODIFY] `Rafeek.Persistence/RafeekDbContext.cs`
- Add: `DbSet<{Entity}> {Entity}s { get; set; }`

> ⛔ **CHECKPOINT 4** — Stop. Wait for approval before writing application layer.

---

## STEP 5 — Application Layer: DTOs & Mapping

**AI Task:** Create the DTO and AutoMapper profile.

### 5.1 — [NEW] `Rafeek.Application/Common/Models/{Entity}Dto.cs`
- Flat DTO exposing only fields needed by the client
- No navigation objects — only primitive types and Guid references
- Add nested DTOs (e.g., `{RelatedEntity}SummaryDto`) if needed

### 5.2 — [NEW] `Rafeek.Application/Mappings/{Entity}Profile.cs`
- Map each Command → Entity for write operations
- Map Entity → Dto for read operations
- Add `.ForMember()` for any non-trivial mappings

> ⛔ **CHECKPOINT 5** — Stop. Wait for approval of DTOs and mapping profile.

---

## STEP 6 — Application Layer: Commands & Queries

Generate **one handler at a time**. Stop after each one for approval.

### 6.1 — Create Command (if requested)

#### [NEW] `Rafeek.Application/Handlers/{Entity}Handlers/Commands/Create{Entity}/Create{Entity}Command.cs`
- Properties matching the fields the client sends
- No navigation objects — only primitive types and GUIDs

#### [NEW] `Rafeek.Application/Handlers/{Entity}Handlers/Commands/Create{Entity}/Create{Entity}CommandHandler.cs`
- Inject `IUnitOfWork _ctx` and `IMapper _mapper`
- Map command → entity using AutoMapper
- Add entity via `_ctx.{Entity}Repository.Add(entity)`
- `await _ctx.SaveChangesAsync(cancellationToken)`
- Return `Unit.Value`

#### [NEW] `Rafeek.Application/Handlers/{Entity}Handlers/Commands/Create{Entity}/Create{Entity}CommandValidator.cs`
- Inject `IStringLocalizer<Messages> _localizer`
- Validate every required field with localized messages
- Use `MustAsync` for uniqueness checks if needed

> ⛔ **CHECKPOINT 6a** — Stop. Wait for approval of Create handler.

---

### 6.2 — Update Command (if requested)

#### [NEW] `...Commands/Update{Entity}/Update{Entity}Command.cs`
- Include `Id` property for identifying the record
- Include only updatable fields

#### [NEW] `...Commands/Update{Entity}/Update{Entity}CommandHandler.cs`
- Fetch entity: `await _ctx.{Entity}Repository.GetByIdAsync(request.Id, cancellationToken)`
- Throw `NotFoundException` if null
- Apply changes manually or via `_mapper.Map(request, entity)`
- `await _ctx.SaveChangesAsync(cancellationToken)`
- Return `bool` or `Unit`

#### [NEW] `...Commands/Update{Entity}/Update{Entity}CommandValidator.cs`

> ⛔ **CHECKPOINT 6b** — Stop. Wait for approval of Update handler.

---

### 6.3 — Delete Command (if requested)

#### [NEW] `...Commands/Delete{Entity}/Delete{Entity}Command.cs`
- Only `Id` property

#### [NEW] `...Commands/Delete{Entity}/Delete{Entity}CommandHandler.cs`
- Fetch entity, throw `NotFoundException` if null
- `_ctx.{Entity}Repository.Remove(entity)` (or soft-delete via flag)
- `await _ctx.SaveChangesAsync(cancellationToken)`

> ⛔ **CHECKPOINT 6c** — Stop. Wait for approval of Delete handler.

---

### 6.4 — Get By Id Query (if requested)

#### [NEW] `...Queries/Get{Entity}ById/Get{Entity}ByIdQuery.cs`
- Property: `public Guid Id { get; set; }`

#### [NEW] `...Queries/Get{Entity}ById/Get{Entity}ByIdQueryHandler.cs`
- Use `.AsNoTracking()` on query
- Use `_mapper.Map<{Entity}Dto>(entity)` or `ProjectTo<>`
- Throw `NotFoundException` if null

> ⛔ **CHECKPOINT 6d** — Stop. Wait for approval of GetById handler.

---

### 6.5 — Get All / Paginated Query (if requested)

#### [NEW] `...Queries/GetAll{Entity}s/GetAll{Entity}sQuery.cs`
- Add `PageNumber`, `PageSize`, optional filter params

#### [NEW] `...Queries/GetAll{Entity}s/GetAll{Entity}sQueryHandler.cs`
- Build queryable with filters applied
- `CountAsync()` for total
- `.Skip().Take().ProjectTo<{Entity}Dto>().ToListAsync()`
- Return `PaginatedResponse<{Entity}Dto>`

> ⛔ **CHECKPOINT 6e** — Stop. Wait for approval of GetAll handler.

---

## STEP 7 — Localization

**AI Task:** Add new localization keys used in validators.

### 7.1 — [MODIFY] `Rafeek.Application/Localization/Messages.ar.resx`
- Add Arabic translations for every new key

### 7.2 — [MODIFY] `Rafeek.Application/Localization/Messages.en.resx` (or equivalent English file)
- Add English translations for every new key

> ⛔ **CHECKPOINT 7** — Stop. Wait for approval of localization entries.

---

## STEP 8 — API Layer

**AI Task:** Add controller and routes.

### 8.1 — [MODIFY] `Rafeek.API/Routes/ApiRoutes.cs`
- Add new static class `{Entity}s` with `Base`, `GetById`, `Create`, `Update`, `Delete` constants

### 8.2 — [NEW] `Rafeek.API/Controllers/Version1/{Entity}Controller.cs`
- Inherits `BaseApiController`
- `[ApiVersion("1.0")]`
- `[Route(ApiRoutes.{Entity}s.Base)]`
- Constructor: `IMediator mediator` + `IStringLocalizer<Messages> localizer`
- One action method per operation, correctly decorated with HTTP verb and `[Authorize]` or `[AllowAnonymous]`
- Every action: `async Task<IActionResult>`, calls `Mediator.Send(...)`, returns via base helper

> ⛔ **CHECKPOINT 8** — Stop. Wait for approval before running migrations.

---

## STEP 9 — Migration

**AI Task:** Generate and apply EF Core migration.

Run in terminal:
```bash
dotnet ef migrations add {YYYYMMDD}_Add{Entity} --project Rafeek.Persistence --startup-project Rafeek.API
dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API
```

Verify:
- Migration file in `Rafeek.Persistence/Migrations/` looks correct
- No unintended drops or renames

> ⛔ **CHECKPOINT 9** — Stop. Confirm migration is applied before testing.

---

## STEP 10 — Final Checklist

Before closing the feature, verify the following:

- [ ] Entity inherits `BaseEntity`
- [ ] Collections use `HashSet<T>`
- [ ] All handler methods are `async`
- [ ] `CancellationToken` passed everywhere
- [ ] No entity exposed directly in API response
- [ ] DTO mapped via AutoMapper profile
- [ ] Validator created for every command
- [ ] Localization keys added to **both** `.resx` files
- [ ] Routes added to `ApiRoutes.cs`
- [ ] `IUnitOfWork` and `UnitOfWork` updated (if new repository)
- [ ] No hardcoded strings in validators or controllers
- [ ] Migration generated and applied
- [ ] Swagger shows new endpoints at correct version

---

## NOTES FOR AI

- Never skip a checkpoint. Always stop and display the checkpoint message.
- Never mix two steps in one response.
- When showing a file, always show the **complete file** including namespace and usings.
- If you discover a missing dependency (e.g., a localization key that already exists), reuse it and document the reuse.
- If business rules require a transaction, use `await _ctx.BeginTransactionAsync(cancellationToken)` pattern.
