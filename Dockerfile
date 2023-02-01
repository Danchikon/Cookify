FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cookify.Api/Cookify.Api.csproj", "Cookify.Api/"]
RUN dotnet restore "Cookify.Api/Cookify.Api.csproj"
COPY . .
WORKDIR "/src/Cookify.Api"
RUN dotnet build "Cookify.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cookify.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cookify.Api.dll"]
