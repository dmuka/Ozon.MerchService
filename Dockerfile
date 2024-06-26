FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Ozon.MerchService/Ozon.MerchService.csproj", "src/Ozon.MerchService/"]
RUN dotnet restore "src/Ozon.MerchService/Ozon.MerchService.csproj"
COPY . .
WORKDIR "/src/Ozon.MerchService"
RUN dotnet build "Ozon.MerchService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.MerchService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY "entrypoint.sh" "app/publish/."

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod -x entrypoint.sh
CMD /bin/bash entrypoint.sh