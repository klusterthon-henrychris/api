FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kluster.Host/Kluster.Host.csproj", "Kluster.Host/"]
COPY ["Kluster.BusinessModule/Kluster.BusinessModule.csproj", "Kluster.BusinessModule/"]
COPY ["Kluster.Shared/Kluster.Shared.csproj", "Kluster.Shared/"]
COPY ["Kluster.NotificationModule/Kluster.NotificationModule.csproj", "Kluster.NotificationModule/"]
COPY ["Kluster.PaymentModule/Kluster.PaymentModule.csproj", "Kluster.PaymentModule/"]
COPY ["Kluster.UserModule/Kluster.UserModule.csproj", "Kluster.UserModule/"]
COPY ["Kluster.MessagingModule/Kluster.MessagingModule.csproj", "Kluster.MessagingModule/"]
RUN dotnet restore "Kluster.Host/Kluster.Host.csproj"
COPY . .
WORKDIR "/src/Kluster.Host"
RUN dotnet build "Kluster.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Kluster.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kluster.Host.dll"]
