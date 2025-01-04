FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orange.Services.ShoppingCartAPI/Orange.Services.ShoppingCartAPI.csproj", "Orange.Services.ShoppingCartAPI/"]
RUN dotnet restore "Orange.Services.ShoppingCartAPI/Orange.Services.ShoppingCartAPI.csproj"
COPY . .
WORKDIR "/src/Orange.Services.ShoppingCartAPI"
RUN dotnet build "Orange.Services.ShoppingCartAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Orange.Services.ShoppingCartAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orange.Services.ShoppingCartAPI.dll"]
