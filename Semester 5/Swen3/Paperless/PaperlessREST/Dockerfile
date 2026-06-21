# .NET 8.0 ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:93b366e510c6cd01cee608447014f7d349cb7ff8809fd0f554aa3772e8587b7e AS base
WORKDIR /app
EXPOSE 8081

# .NET 8.0 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:d80fdd84f7e18eea12f8e45c52914f1353395009c95c41197178ea19944e6d48 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["PaperlessREST/PaperlessREST.csproj", "PaperlessREST/"]
COPY ["Contract/Contract.csproj", "Contract/"]
RUN dotnet restore "PaperlessREST/PaperlessREST.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/PaperlessREST"
RUN dotnet build "PaperlessREST.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "PaperlessREST.csproj" -c Release -r linux-x64 --no-self-contained -o /app/publish

# Create the final image to run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY PaperlessREST/rest-appsettings.json /app/rest-appsettings.json
ENTRYPOINT ["dotnet", "PaperlessREST.dll"]
