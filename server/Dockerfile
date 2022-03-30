FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY src/PriceTracker.Api/PriceTracker.Api.csproj src/PriceTracker.Api/
COPY src/PriceTracker.Infrastructure/PriceTracker.Infrastructure.csproj src/PriceTracker.Infrastructure/
COPY src/PriceTracker.Core/PriceTracker.Core.csproj src/PriceTracker.Core/
COPY test/PriceTracker.Test/PriceTracker.Test.csproj test/PriceTracker.Test/
COPY PriceTracker.sln .

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as run

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "PriceTracker.Api.dll"]
