FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Kluster.Host/Kluster.Host.csproj", "Kluster.Host/"]
RUN dotnet restore "Kluster.Host/Kluster.Host.csproj"
COPY . .
WORKDIR "/src/Kluster.Host"
RUN dotnet build "Kluster.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kluster.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kluster.Host.dll"]
