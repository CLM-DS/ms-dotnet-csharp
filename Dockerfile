FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS publish
WORKDIR /src
COPY . .
WORKDIR /src/microservices/Basket/Basket.API
RUN dotnet publish -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as release
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
