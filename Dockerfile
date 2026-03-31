# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

# Copy solution + project files first for layer-cached restore
COPY TaxCalculator.sln ./
COPY src/TaxCalculator.Domain/TaxCalculator.Domain.csproj              src/TaxCalculator.Domain/
COPY src/TaxCalculator.Application/TaxCalculator.Application.csproj    src/TaxCalculator.Application/
COPY src/TaxCalculator.Infrastructure/TaxCalculator.Infrastructure.csproj src/TaxCalculator.Infrastructure/
COPY src/TaxCalculator.API/TaxCalculator.API.csproj                     src/TaxCalculator.API/

RUN dotnet restore

# Copy everything else and publish
COPY . .
RUN dotnet publish src/TaxCalculator.API/TaxCalculator.API.csproj \
    -c Release -o /app/publish --no-restore

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TaxCalculator.API.dll"]
