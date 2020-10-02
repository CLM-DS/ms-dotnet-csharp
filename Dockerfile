FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY "microservices/Basket/Basket.sln" "microservices/Basket/Basket.sln"

COPY "events/EventBus/EventBus.csproj" "events/EventBus/EventBus.csproj"
COPY "events/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "events/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "events/EventBusServiceBus/EventBusServiceBus.csproj" "events/EventBusServiceBus/EventBusServiceBus.csproj"
COPY "events/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "events/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "microservices/Basket/Basket.API/Basket.API.csproj" "microservices/Basket/Basket.API/Basket.API.csproj"
COPY "microservices/Basket/Basket.FunctionalTests/Basket.FunctionalTests.csproj" "microservices/Basket/Basket.FunctionalTests/Basket.FunctionalTests.csproj"
COPY "microservices/Basket/Basket.UnitTests/Basket.UnitTests.csproj" "microservices/Basket/Basket.UnitTests/Basket.UnitTests.csproj"

RUN dotnet restore "microservices/Basket/Basket.sln"

COPY . .
WORKDIR /src/microservices/Basket/Basket.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build as unittest
WORKDIR /src/microservices/Basket/Basket.UnitTests

FROM build as functionaltest
WORKDIR /src/microservices/Basket/Basket.FunctionalTests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
