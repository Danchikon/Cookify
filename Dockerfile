FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS builder
WORKDIR /app

COPY . .

RUN dotnet restore "src/Cookify.Api/Cookify.Api.csproj"
RUN dotnet publish "src/Cookify.Api/Cookify.Api.csproj" -c Release -o /dist 

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS runner
WORKDIR /app

COPY --from=builder /dist /app

RUN apt update && apt install -y libc6 libc6-dev libgtk2.0-0 libnss3 libatk-bridge2.0-0 libx11-xcb1 libxcb-dri3-0 libdrm-common libgbm1 libasound2 libappindicator3-1 libxrender1 libfontconfig1 libxshmfence1 libgdiplus libva-dev
RUN chmod +rwx /app/runtimes/linux-x64/native/IronCefSubprocess

ENTRYPOINT ["dotnet", "Cookify.Api.dll"]
