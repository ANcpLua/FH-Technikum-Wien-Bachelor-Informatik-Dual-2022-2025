FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:63ebabdcde24cc8134304a6f50719cf1e22bb4e9f7148ac4ef967c7680187356 AS build
WORKDIR /src

COPY *.sln ./
COPY API/*.csproj API/
COPY BL/*.csproj BL/
COPY DAL/*.csproj DAL/
COPY UI/*.csproj UI/

RUN dotnet restore "API/API.csproj"
RUN dotnet restore "UI/UI.csproj"

COPY . .

RUN dotnet publish "API/API.csproj" -c Release -o /app/api/publish
RUN dotnet publish "UI/UI.csproj" -c Release -o /app/ui/publish \
    --no-restore \
    /p:ServiceWorkerForce=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:e78fda31142e28746a6908e288e0d40346793f691ca99d8d150bcbe95c0ef035 AS api
WORKDIR /app

COPY --from=build /app/api/publish ./

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libgdiplus && \
    rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "API.dll"]

FROM nginx:alpine@sha256:20316569d8f81a160065d7d2a5eeffc7ca97d79022462ee255fd23fa103a6b5c AS ui
WORKDIR /usr/share/nginx/html

COPY --from=build /app/ui/publish/wwwroot ./
COPY nginx.conf /etc/nginx/nginx.conf

RUN mkdir -p js && \
    touch service-worker.published.js && \
    touch service-worker.js && \
    chown -R nginx:nginx /usr/share/nginx/html && \
    chmod -R 755 /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
