# Rafeek — AGENTS.md

.NET 8 Clean Architecture + CQRS academic management system.

## Architecture

```
Domain  ←  Application  ←  Infrastructure  ←  API
(Persistence depends only on Domain + Application)
```

- **Domain**: Entities (inherit `BaseEntity`), Enums, Repository interfaces.
- **Persistence** (`Rafeek.Persistence`): Two DbContexts with separate migration histories — `RafeekDbContext` (`dbo` schema, `Migrations/Rafeek/`) and `RafeekIdentityDbContext` (`auth` schema, `Migrations/Identity/`). References `Rafeek.Application`, not `Rafeek.Infrastructure`.
- **Application**: MediatR CQRS handlers, DTOs, FluentValidation validators, AutoMapper profiles under `Handlers/{Entity}Handlers/{Commands|Queries}/{Action}{Entity}/`.
- **Infrastructure**: Repository impls, JWT + Identity, Email (MailKit), file validation, AI HTTP client. Registers `IUnitOfWork`/`IIdentityUnitOfWork`.
- **API**: Controllers inherit `BaseApiController`, routes are constants in `Routes/ApiRoutes.cs` (`api/v{version:apiVersion}/{kebab-resource}`).

All data access in handlers goes through `IUnitOfWork` — never inject DbContext directly.

## Startup behavior

`Program.cs:300-319` runs `RafeekDbSeeder.SeedAsync()` on every startup after `MigrateAsync()`. Each seeder block checks `AnyAsync()` before inserting — but beware:
- **Courses**: the `else` branch previously called `UpdateRange(courses)` unconditionally every startup, generating UPDATE statements for all rows. Fixed to only update when `WeeklyLectureHours == 0`.
- **Enrollments & Grades**: the seeder previously reset local lists to empty when data existed, causing re-insertion on every run.

## Commands

```powershell
# Build entire solution
dotnet build Rafeek.sln

# Run API (listens on Kestrel, ports from launchSettings)
dotnet run --project Rafeek.API

# Add a migration (prefix with year+month)
dotnet ef migrations add 202607_{Name} --project Rafeek.Persistence --startup-project Rafeek.API

# Update database
dotnet ef database update --project Rafeek.Persistence --startup-project Rafeek.API

# Generate seed faker script (requires: dotnet tool install -g dotnet-script)
dotnet script Rafeek.Persistence/gen.csx
```

## Key conventions

- **Serialization**: Newtonsoft.Json with `CamelCasePropertyNamesContractResolver` + `StringEnumConverter`. Not System.Text.Json.
- **Localization**: Default culture is `ar`. All user-facing messages use `IStringLocalizer<Messages>` with keys from `LocalizationKeys.cs`. Add both `ar` and `en` entries in `.resx` files.
- **Collections**: Use `new HashSet<T>()`, not `List<T>` for navigation properties in entities.
- **Validation**: One `AbstractValidator<TCommand>` per command, co-located. Never validate manually in handlers.
- **Error handling**: Throw typed exceptions (`NotFoundException`, `BadRequestException`, `UnauthorizedException`, `ValidationException`). Pipeline handles the rest.
- **Soft delete**: `BaseEntity.IsDeleted` + `DeletedAt`/`DeletedBy` — respect in queries.
- **Performance**: `.AsNoTracking()` on all read-only queries. `ProjectTo<TDto>()` preferred over loading + mapping.
- **API versioning**: Default v1.0, controllers decorated with `[ApiVersion("1.0")]`, format `api/v{version:apiVersion}/...`.
- **Seeder idempotency**: Every block guards on `AnyAsync()` — never assume a table is empty without checking.

## Config & environment quirks

- `Program.cs:64-79` clears default config sources and rebuilds from `appsettings.json` + `appsettings.{env}.json` only.
- `EnableSensitiveDataLogging()` is on unconditionally in `Rafeek.Persistence/DependencyInjection.cs:38` — not just in dev.
- Connection strings (`RafeekConnectionString`, `RafeekIdentityConnectionString`) point to the same remote DB.
- `OpenCode.json` sets `"edit": "ask"` — edits require confirmation.
- `permission_matrix.md` documents role-based access (Admin, Staff, Professor, Mentor, Student) in Arabic.

## Testing

No test project exists. Do not assume a test framework or run test commands.
