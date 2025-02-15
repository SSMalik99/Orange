FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Oragne.GatewaySolution/Oragne.GatewaySolution.csproj", "Oragne.GatewaySolution/"]
RUN dotnet restore "Oragne.GatewaySolution/Oragne.GatewaySolution.csproj"
COPY . .
WORKDIR "/src/Oragne.GatewaySolution"
RUN dotnet build "Oragne.GatewaySolution.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Oragne.GatewaySolution.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Oragne.GatewaySolution.dll"]
