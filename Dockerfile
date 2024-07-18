FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 2291
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Wallet.csproj", "./"]
RUN dotnet restore "Wallet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Wallet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wallet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallet.dll"]