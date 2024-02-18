FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Voter/Voter.csproj", "Voter/"]
RUN dotnet restore "Voter/Voter.csproj"
COPY . .
WORKDIR "/src/Voter"
RUN dotnet build "Voter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Voter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Voter.dll"]
