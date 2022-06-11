#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Auction.API/Auction.API.csproj", "Auction.API/"]
RUN dotnet restore "Auction.API/Auction.API.csproj"
COPY . .
WORKDIR "/src/Auction.API"
RUN dotnet build "Auction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auction.API.dll"]