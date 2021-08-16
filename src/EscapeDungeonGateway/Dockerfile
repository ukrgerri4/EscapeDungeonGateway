#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/EscapeDungeonGateway/EscapeDungeonGateway.csproj", "src/EscapeDungeonGateway/"]
RUN dotnet restore "src/EscapeDungeonGateway/EscapeDungeonGateway.csproj"
COPY src/EscapeDungeonGateway/ ./EscapeDungeonGateway
WORKDIR "/src/EscapeDungeonGateway"
RUN dotnet build "EscapeDungeonGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EscapeDungeonGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EscapeDungeonGateway.dll"]