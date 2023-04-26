#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY ["PropertySearchApp/Views", "./Views/"]
EXPOSE $PORT
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["PropertySearchApp/PropertySearchApp.csproj", "PropertySearchApp/"]

RUN dotnet restore "PropertySearchApp/PropertySearchApp.csproj"
COPY . .
WORKDIR "/src/PropertySearchApp"
RUN dotnet build "PropertySearchApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PropertySearchApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropertySearchApp.dll"]