FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["bradjolicoeur.web/bradjolicoeur.web.csproj", "bradjolicoeur.web/"]
COPY ["bradjolicoeur.core/bradjolicoeur.core.csproj", "bradjolicoeur.core/"]
RUN dotnet restore "bradjolicoeur.web/bradjolicoeur.web.csproj"
COPY . .
WORKDIR "/src/bradjolicoeur.web"
RUN dotnet build "bradjolicoeur.web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "bradjolicoeur.web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "bradjolicoeur.web.dll"]
