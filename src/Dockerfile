#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY ["PropertySearch.Api/Views", "./Views/"]
EXPOSE $PORT
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["PropertySearch.Api/PropertySearch.Api.csproj", "PropertySearch.Api/"]

RUN dotnet restore "PropertySearch.Api/PropertySearch.Api.csproj"
COPY . .
WORKDIR "/src/PropertySearch.Api"
RUN dotnet build "PropertySearch.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PropertySearch.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropertySearch.Api.dll"]