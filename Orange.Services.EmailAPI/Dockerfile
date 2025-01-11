FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orange.Services.EmailAPI/Orange.Services.EmailAPI.csproj", "Orange.Services.EmailAPI/"]
RUN dotnet restore "Orange.Services.EmailAPI/Orange.Services.EmailAPI.csproj"
COPY . .
WORKDIR "/src/Orange.Services.EmailAPI"
RUN dotnet build "Orange.Services.EmailAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Orange.Services.EmailAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orange.Services.EmailAPI.dll"]
