FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:d80fdd84f7e18eea12f8e45c52914f1353395009c95c41197178ea19944e6d48 AS build
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

FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:93b366e510c6cd01cee608447014f7d349cb7ff8809fd0f554aa3772e8587b7e AS api
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
