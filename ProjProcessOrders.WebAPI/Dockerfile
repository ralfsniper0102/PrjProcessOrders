FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ProjProcessOrders.WebAPI/ProjProcessOrders.WebAPI.csproj", "ProjProcessOrders.WebAPI/"]
COPY ["ProjProcessOrders.UseCase/ProjProcessOrders.UseCase.csproj", "ProjProcessOrders.UseCase/"]
COPY ["ProjProcessOrders.Domain/ProjProcessOrders.Domain.csproj", "ProjProcessOrders.Domain/"]
COPY ["ProjProcessOrders.Exception/ProjProcessOrders.Exception.csproj", "ProjProcessOrders.Exception/"]
COPY ["ProjProcessOrders.Localization/ProjProcessOrders.Localization.csproj", "ProjProcessOrders.Localization/"]
COPY ["ProjProcessOrders.Messaging/ProjProcessOrders.Messaging.csproj", "ProjProcessOrders.Messaging/"]
RUN dotnet restore "./ProjProcessOrders.WebAPI/ProjProcessOrders.WebAPI.csproj"

COPY . .
WORKDIR "/src/ProjProcessOrders.WebAPI"
RUN dotnet build "./ProjProcessOrders.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./ProjProcessOrders.WebAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjProcessOrders.WebAPI.dll"]