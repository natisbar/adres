FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["adres.csproj", "./"]
RUN dotnet restore "adres.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "adres.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "adres.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# # Ejecutar la migración de la base de datos
# RUN dotnet ef database update

ENTRYPOINT ["dotnet", "adres.dll"]