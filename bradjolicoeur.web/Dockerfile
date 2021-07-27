#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["bradjolicoeur.web/bradjolicoeur.web.csproj", "bradjolicoeur.web/"]
COPY ["bradjolicoeur.core/bradjolicoeur.core.csproj", "bradjolicoeur.core/"]
RUN dotnet restore "bradjolicoeur.web/bradjolicoeur.web.csproj"
COPY . .
WORKDIR "/src/bradjolicoeur.web"
RUN dotnet build "bradjolicoeur.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bradjolicoeur.web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "bradjolicoeur.web.dll"]