FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MergeInterval/MergeInterval.csproj", "MergeInterval/"]
RUN dotnet restore "MergeInterval/MergeInterval.csproj"
COPY . .
WORKDIR "/src/MergeInterval"
RUN dotnet build "MergeInterval.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MergeInterval.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MergeInterval.dll"]
