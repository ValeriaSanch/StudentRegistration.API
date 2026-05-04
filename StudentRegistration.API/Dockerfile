# syntax=docker/dockerfile:1

# ── Build stage ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore cache
COPY ["StudentRegistration.API/StudentRegistration.API.csproj", "StudentRegistration.API/"]
COPY ["StudentRegistration.Domain/StudentRegistration.Domain.csproj", "StudentRegistration.Domain/"]
COPY ["StudentRegistration.Application/StudentRegistration.Application.csproj", "StudentRegistration.Application/"]
COPY ["StudentRegistration.Infrastructure/StudentRegistration.Infrastructure.csproj", "StudentRegistration.Infrastructure/"]
COPY ["StudentRegistration.Presentation/StudentRegistration.Presentation.csproj", "StudentRegistration.Presentation/"]

RUN dotnet restore "StudentRegistration.API/StudentRegistration.API.csproj"

COPY . .
WORKDIR "/src/StudentRegistration.API"
RUN dotnet build "StudentRegistration.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ── Publish stage ─────────────────────────────────────────────────────────────
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StudentRegistration.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ── Runtime stage ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentRegistration.API.dll"]
