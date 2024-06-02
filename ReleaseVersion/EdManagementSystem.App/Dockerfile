FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EdManagementSystem.App/EdManagementSystem.App.csproj", "EdManagementSystem.App/"]
COPY ["EdManagementSystem.DataAccess/EdManagementSystem.DataAccess.csproj", "EdManagementSystem.DataAccess/"]
RUN dotnet restore "./EdManagementSystem.App/EdManagementSystem.App.csproj"
COPY . .
WORKDIR "/src/EdManagementSystem.App"
RUN dotnet build "./EdManagementSystem.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EdManagementSystem.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EdManagementSystem.App.dll"]