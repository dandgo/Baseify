FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Baseify.Api/Baseify.Api.csproj", "Baseify.Api/"]
COPY ["Baseify.Application/Baseify.Application.csproj", "Baseify.Application/"]
COPY ["Baseify.Domain/Baseify.Domain.csproj", "Baseify.Domain/"]
COPY ["Baseify.Infrastructure/Baseify.Infrastructure.csproj", "Baseify.Infrastructure/"]
RUN dotnet restore "./Baseify.Api/./Baseify.Api.csproj"

COPY . .
WORKDIR "/src/Baseify.Api"
RUN dotnet build "./Baseify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Baseify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baseify.Api.dll"]