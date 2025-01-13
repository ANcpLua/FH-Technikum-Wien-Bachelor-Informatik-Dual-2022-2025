# .NET 8.0 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BUILD_CONFIG=Release
WORKDIR /App

# Copy the project files and restore dependencies
COPY ["PaperlessServices/PaperlessServices.csproj", "PaperlessServices/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY . .
RUN dotnet restore "PaperlessServices/PaperlessServices.csproj"

# Build and publish the application
RUN dotnet publish -c $BUILD_CONFIG -r linux-x64 --no-self-contained -o out "PaperlessServices/PaperlessServices.csproj"

# .NET 8.0 ASP.NET runtime as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

# Install necessary dependencies
RUN apt-get update -y && \
    apt-get install -y \
    libc6-dev \
    libleptonica-dev \
    wget \
    gnupg2 \
    apt-transport-https \
    software-properties-common \
    curl

# Add Tesseract OCR 5 repository and install Tesseract and its dependencies
RUN echo "deb https://notesalexp.org/tesseract-ocr5/$(lsb_release -cs)/ $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/notesalexp.list > /dev/null && \
    wget -O - https://notesalexp.org/debian/alexp_key.asc | apt-key add - && \
    apt-get update -y && \
    apt-get install -y \
    tesseract-ocr \
    tesseract-ocr-eng \
    ghostscript \
    libgs-dev \
    imagemagick \
    libmagickwand-dev

# Set up Tesseract data directories and permissions
RUN mkdir -p /usr/share/tesseract-ocr/4.00/tessdata && \
    mkdir -p /App/tessdata && \
    cp /usr/share/tesseract-ocr/*/tessdata/eng.traineddata /App/tessdata/ && \
    cp /usr/share/tesseract-ocr/*/tessdata/eng.traineddata /usr/share/tesseract-ocr/4.00/tessdata/ && \
    chmod -R 755 /App/tessdata && \
    chmod -R 755 /usr/share/tesseract-ocr/4.00/tessdata

# Verify Tesseract installation
RUN tesseract --version && \
    ls -la /App/tessdata && \
    ls -la /usr/share/tesseract-ocr/4.00/tessdata

# Handle architecture-specific setup
ARG TARGETPLATFORM
RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
        export ARCHITECTURE="x86_64"; \
        export CPU_BIT="64"; \
    elif [ "$TARGETPLATFORM" = "linux/arm/v7" ]; then \
        export ARCHITECTURE="arm"; \
        export CPU_BIT="32"; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
        export ARCHITECTURE="aarch64"; \
    else \
        export ARCHITECTURE="x86"; \
        export CPU_BIT="32"; \
    fi

# Create necessary symbolic links for libdl.so
RUN mkdir -p /App/runtimes/linux-x64/native/ && \
    ln -s /lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/x86_64-linux-gnu/libdl.so && \
    ln -s /lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/libdl.so && \
    ln -s /lib/x86_64-linux-gnu/libdl.so.2 /App/runtimes/linux-x64/native/libdl.so

# Handle additional architecture-specific symbolic links
RUN if [ "$CPU_BIT" != "64" ]; then \
        if [ "$ARCHITECTURE" != "x86_64" ]; then \
            ln -s /usr/lib/aarch64-linux-gnu /usr/lib/x86_64-linux-gnu; \
        fi \
    fi

# Set up additional necessary symbolic links
RUN mkdir -p /App/x64 && \
    ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 /App/x64/libleptonica-1.82.0.so && \
    ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5 /App/x64/libtesseract50.so && \
    ln -s /usr/lib/x86_64-linux-gnu/* /App/x64/

# Copy the application configuration file
COPY PaperlessServices/service-appsettings.json /app/service-appsettings.json

# Set environment variables for Tesseract
ENV TESSDATA_PREFIX=/App/tessdata/
ENV TESSERACT_PREFIX=/App/tessdata/

# Expose the application port
EXPOSE 8082

# Define the entry point for the application
ENTRYPOINT ["dotnet", "PaperlessServices.dll"]
