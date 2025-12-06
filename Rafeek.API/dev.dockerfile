# Step 01: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and all project files first (for better layer caching)
COPY ["Rafeek.sln", "./"]
COPY ["Rafeek.API/Rafeek.API.csproj", "Rafeek.API/"]
COPY ["Rafeek.Application/Rafeek.Application.csproj", "Rafeek.Application/"]
COPY ["Rafeek.Domain/Rafeek.Domain.csproj", "Rafeek.Domain/"]
COPY ["Rafeek.Infrastructure/Rafeek.Infrastructure.csproj", "Rafeek.Infrastructure/"]
COPY ["Rafeek.Persistence/Rafeek.Persistence.csproj", "Rafeek.Persistence/"]

# Restore dependencies
RUN dotnet restore "Rafeek.API/Rafeek.API.csproj"

# Copy all source code
COPY . .

# Step 02: Publish Stage
WORKDIR /src/Rafeek.API
RUN dotnet publish "Rafeek.API.csproj" -c Release -o /app/publish --no-restore

# Step 03: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port (Railway uses dynamic PORT)
EXPOSE 8080

# Set environment variables
# Railway provides PORT env variable, fallback to 8080 for local dev
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application with dynamic port binding
# Uses PORT from environment (Railway) or defaults to 8080
CMD ["/bin/sh", "-c", "dotnet Rafeek.API.dll --urls=http://+:${PORT:-8080}"]