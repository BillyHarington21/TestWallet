# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TestWallet/*.csproj TestWallet/
COPY TestWallet.Application/*.csproj TestWallet.Application/
COPY TestWallet.Domain/*.csproj TestWallet.Domain/
COPY TestWallet.Infrastructure/*.csproj TestWallet.Infrastructure/

RUN dotnet restore TestWallet/TestWallet.API.csproj

COPY . .

WORKDIR /src/TestWallet
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestWallet.API.dll"]
