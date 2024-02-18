FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RateLimiter/RateLimiter.csproj", "RateLimiter/"]
RUN dotnet restore "RateLimiter/RateLimiter.csproj"
COPY . .
WORKDIR "/src/RateLimiter"
RUN dotnet build "RateLimiter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RateLimiter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RateLimiter.dll"]
