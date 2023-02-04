FROM mcr.microsoft.com/dotnet/sdk:6.0 AS builder
WORKDIR /app

COPY . .

RUN dotnet restore "src/Cookify.Api/Cookify.Api.csproj"
RUN dotnet publish "src/Cookify.Api/Cookify.Api.csproj" -c Release -o /dist 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runner

COPY --from=builder /dist .

ENTRYPOINT ["dotnet", "Cookify.Api.dll"]
