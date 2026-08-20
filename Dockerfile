# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["CRN.Product.API.csproj", "./"]

RUN dotnet restore "CRN.Product.API.csproj"

COPY . .

RUN dotnet publish "CRN.Product.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CRN.Product.API.dll"]