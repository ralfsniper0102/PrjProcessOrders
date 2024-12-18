FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ProjProcessOrders.ProcessingAPI/ProjProcessOrders.ProcessingAPI.csproj", "ProjProcessOrders.ProcessingAPI/"]
COPY ["ProjProcessOrders.Application/ProjProcessOrders.Application.csproj", "ProjProcessOrders.Application/"]
COPY ["ProjProcessOrders.Domain/ProjProcessOrders.Domain.csproj", "ProjProcessOrders.Domain/"]
COPY ["ProjProcessOrders.Exception/ProjProcessOrders.Exception.csproj", "ProjProcessOrders.Exception/"]
COPY ["ProjProcessOrders.UseCase/ProjProcessOrders.UseCase.csproj", "ProjProcessOrders.UseCase/"]
COPY ["ProjProcessOrders.Localization/ProjProcessOrders.Localization.csproj", "ProjProcessOrders.Localization/"]
COPY ["ProjProcessOrders.Composition/ProjProcessOrders.Composition.csproj", "ProjProcessOrders.Composition/"]
COPY ["ProjProcessOrders.Infrastructure/ProjProcessOrders.Infrastructure.csproj", "ProjProcessOrders.Infrastructure/"]
COPY ["ProjProcessOrders.Persistence/ProjProcessOrders.Persistence.csproj", "ProjProcessOrders.Persistence/"]
COPY ["ProjProcessOrders.Messaging/ProjProcessOrders.Messaging.csproj", "ProjProcessOrders.Messaging/"]
RUN dotnet restore "./ProjProcessOrders.ProcessingAPI/ProjProcessOrders.ProcessingAPI.csproj"

COPY . .
WORKDIR "/src/ProjProcessOrders.ProcessingAPI"
RUN dotnet publish "./ProjProcessOrders.ProcessingAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

EXPOSE 5000
WORKDIR /app

COPY --from=build /app/publish . 

ENTRYPOINT ["dotnet", "ProjProcessOrders.ProcessingAPI.dll"]
