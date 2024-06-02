FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8001

ENV ASPNETCORE_ENVIRONMENT=Development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EdManagementSystem.API/EdManagementSystem.API.csproj", "EdManagementSystem.API/"]
COPY ["EdManagementSystem.DataAccess/EdManagementSystem.DataAccess.csproj", "EdManagementSystem.DataAccess/"]
RUN dotnet restore "./EdManagementSystem.API/EdManagementSystem.API.csproj"
COPY . .
WORKDIR "/src/EdManagementSystem.API"
RUN dotnet build "./EdManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EdManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EdManagementSystem.API.dll"]