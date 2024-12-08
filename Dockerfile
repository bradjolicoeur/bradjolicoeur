FROM mcr.microsoft.com/dotnet/aspnet:9.0-jammy-chiseled-extra AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0-jammy AS build
WORKDIR /src

COPY . .

FROM build AS publish
RUN dotnet publish "bradjolicoeur.sln" -c Release -o /app 

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "bradjolicoeur.web.dll"]
