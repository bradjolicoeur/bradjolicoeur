FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY . .

RUN dotnet restore -c Release /app

RUN dotnet build -c Release -o /app --no-restore

FROM build AS publish
RUN dotnet publish -c Release -o /app --no-restore --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "bradjolicoeur.web.dll"]
