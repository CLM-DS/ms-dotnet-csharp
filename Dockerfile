FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS publish
WORKDIR /src
COPY . .
ENV SOLUTION_DIRECTORY=./microservices/Basket
RUN echo ${SOLUTION_DIRECTORY}
RUN dotnet publish -c Release --no-restore ${SOLUTION_DIRECTORY}/Basket.API -o /app 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as release
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
