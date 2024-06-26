﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Ozon.MerchService/Ozon.MerchService.csproj", "src/Ozon.MerchService/"]
RUN dotnet restore "src/Ozon.MerchService/Ozon.MerchService.csproj"
COPY . .
WORKDIR "/src/src/Ozon.MerchService"
RUN dotnet build "Ozon.MerchService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.MerchService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR "app/publish/"
COPY "entrypoint.sh" .
RUN chmod +x entrypoint.sh

FROM base AS final
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

CMD ["/bin/bash", "entrypoint.sh"]
