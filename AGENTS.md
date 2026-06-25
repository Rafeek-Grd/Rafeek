# Rafeek — AGENTS.md

.NET 8 Clean Architecture + CQRS academic management system.

## Architecture

```
Domain  ←  Application  ←  Infrastructure  ←  API
(Persistence depends only on Domain + Application)
```

- **Domain**: Entities (inherit `BaseEntity`), Enums, Repository interfaces. Zero deps.
- **Persistence**: Two DbContexts — `RafeekDbContext` (app data, `dbo` schema) and `RafeekIdentityDbContext` (auth, `auth` schema). Two distinct migration histories in `Migrations/Rafeek/` and `Migrations/Identity/`.
- **Application**: MediatR CQRS handlers, DTOs, FluentValidation validators, AutoMapper profiles in vertical slice folders under `Handlers/{Entity}Handlers/{Commands|Queries}/{Action}{Entity}/`.
- **Infrastructure**: Repository impls, JWT + Identity, Email (MailKit), file validation, AI service HTTP client.
- **API**: Controllers inherit `BaseApiController`, routes are constants in `Routes/ApiRoutes.cs`, format `api/v{version:apiVersion}/{kebab-resource}`.

All data access in handlers goes through `IUnitOfWork` — never inject DbContext directly.

## Commands

```powershell
# Build entire solution
dotnet build Rafeek.sln

# Run API (listens on Kestrel, ports from launchSettings)
dotnet run --project Rafeek.API

# Add a migration (specify year+month prefix)
dotnet ef migrations add 202406_{Name} --project Rafeek.Persistence --startup-project Rafeek.API

# Update database
dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API

# Generate seed faker script
dotnet script Rafeek.Persistence/gen.csx
```

## Conventions

- **Serialization**: Newtonsoft.Json with `CamelCasePropertyNamesContractResolver` + `StringEnumConverter`. Not System.Text.Json.
- **Localization**: Default culture is `ar`. All user-facing messages use `IStringLocalizer<Messages>` with keys from `LocalizationKeys.cs`. Add both `ar` and `en` entries in `.resx` files.
- **Collections**: Use `new HashSet<T>()`, not `List<T>`.
- **Validation**: One `AbstractValidator<TCommand>` per command, co-located. Never validate manually in handlers.
- **Error handling**: Throw typed exceptions (`NotFoundException`, `BadRequestException`, `UnauthorizedException`, `ValidationException`). Pipeline handles the rest.
- **API versioning**: Default v1.0, controllers decorated with `[ApiVersion("1.0")]`.
- **Soft delete**: `BaseEntity.IsDeleted` + `DeletedAt`/`DeletedBy` — respect in queries.
- **Performance**: `.AsNoTracking()` on all read-only queries. `ProjectTo<TDto>()` preferred over loading + mapping.

## Testing

No test project exists. Do not assume a test framework or run test commands.

## Important

- `DbContextOptionsBuilder.EnableSensitiveDataLogging()` is on in dev — do not ship to prod.
- Connection strings in `appsettings.Development.json` currently point to a shared remote database (`databaseasp.net`). The `RafeekConnectionString` and `RafeekIdentityConnectionString` both point to the same database.
- `appsettings.json` is minimal; secrets live in `appsettings.Development.json` / `appsettings.Live.json`.
- `OpenCode.json` sets `"edit": "ask"` — edits require confirmation.
- Existing agent instruction files: `.agents/rules/rafeek.md` (system rules) and `.agents/workflows/rafeekworkflow.md` (feature workflow with checkpoints). Refer to those for detailed coding conventions.
- `permission_matrix.md` documents role-based access (Admin, Staff, Professor, Mentor, Student) in Arabic.
