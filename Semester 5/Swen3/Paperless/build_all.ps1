# Define the required .NET SDK version
$requiredVersion = "8.0"

# Function to check if the required .NET SDK is installed
function Is-DotNetSdkInstalled {
    $sdks = & dotnet --list-sdks 2>$null
    return $sdks -match "^$requiredVersion\."
}

# Function to install the .NET SDK using the official install script
function Install-DotNetSdk {
    $installScriptUrl = "https://dot.net/v1/dotnet-install.ps1"
    $installScriptPath = "$env:TEMP\dotnet-install.ps1"
    
    # Download the install script
    Invoke-WebRequest -Uri $installScriptUrl -OutFile $installScriptPath
    
    # Execute the install script to install the required .NET SDK version
    & $installScriptPath -Channel $requiredVersion -InstallDir "$env:ProgramFiles\dotnet"
}

# Function to setup development certificates
function Setup-DevCertificates {
    Write-Host "Setting up HTTPS development certificates..." -ForegroundColor Yellow
    
    # Remove existing dev certs
    dotnet dev-certs https --clean
    
    # Create and trust the new certificate
    dotnet dev-certs https --trust
    
    Write-Host "Development certificates setup completed" -ForegroundColor Green
}

# Function to run database migrations
function Update-Database {
    param (
        [string]$Project,
        [string]$StartupProject
    )
    
    Write-Host "Running database migrations for $Project..." -ForegroundColor Yellow
    
    try {
        # Don't try to install EF tools if they're already installed
        try {
            dotnet tool install --global dotnet-ef
        } catch {
            Write-Host "EF Core tools already installed" -ForegroundColor Cyan
        }
        
        # Run the migrations from the PostgreSQL project
        Set-Location $Project
        dotnet ef database update --context PaperlessDbContext --startup-project ..\$StartupProject
        
        Write-Host "Database migrations completed successfully" -ForegroundColor Green
    }
    catch {
        Write-Host "Error running migrations: $_" -ForegroundColor Red
        exit 1
    }
    finally {
        Set-Location $solutionDir
    }
}

# Check if the required .NET SDK is installed
if (-not (Is-DotNetSdkInstalled)) {
    Write-Output ".NET SDK version $requiredVersion not found. Installing..."
    Install-DotNetSdk
    
    # Verify installation
    if (Is-DotNetSdkInstalled) {
        Write-Output ".NET SDK version $requiredVersion installed successfully."
    } else {
        Write-Error "Failed to install .NET SDK version $requiredVersion."
        exit 1
    }
} else {
    Write-Output ".NET SDK version $requiredVersion is already installed."
}

# Get the directory of the current script
$solutionDir = Split-Path $MyInvocation.MyCommand.Path

# Change to the solution directory
Set-Location $solutionDir

# Update all .csproj files to target .NET 8.0 and set Docker settings
Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $projectPath = $_.FullName
    Write-Host "Processing project: $projectPath" -ForegroundColor Cyan
    
    # Load the project file
    [xml]$xml = Get-Content $projectPath
    
    # Get first PropertyGroup or create one if it doesn't exist
    $propertyGroup = $xml.Project.PropertyGroup
    if (-not $propertyGroup) {
        $propertyGroup = $xml.CreateElement("PropertyGroup")
        [void]$xml.Project.AppendChild($propertyGroup)
    } elseif ($propertyGroup -is [array]) {
        $propertyGroup = $propertyGroup[0]
    }
    
    # Update TargetFramework
    $targetFramework = $propertyGroup.SelectSingleNode("TargetFramework")
    if (-not $targetFramework) {
        $targetFramework = $xml.CreateElement("TargetFramework")
        [void]$propertyGroup.AppendChild($targetFramework)
    }
    $targetFramework.InnerText = "net8.0"
    
    # Update DockerDefaultTargetOS
    $dockerDefaultTargetOS = $propertyGroup.SelectSingleNode("DockerDefaultTargetOS")
    if (-not $dockerDefaultTargetOS) {
        $dockerDefaultTargetOS = $xml.CreateElement("DockerDefaultTargetOS")
        [void]$propertyGroup.AppendChild($dockerDefaultTargetOS)
    }
    $dockerDefaultTargetOS.InnerText = "Linux"
    
    # Remove specific elements if they exist
    @("RuntimeIdentifiers", "PublishSingleFile", "SelfContained") | ForEach-Object {
        $element = $xml.Project.PropertyGroup.SelectSingleNode("//$_")
        if ($element) {
            [void]$element.ParentNode.RemoveChild($element)
        }
    }
    
    # Save the updated .csproj file
    $xml.Save($projectPath)
    Write-Host "Updated project file successfully" -ForegroundColor Green
}

# Setup development certificates
Setup-DevCertificates

# Create tessdata directory in your test project and download language data
Write-Host "Setting up Tesseract OCR data..." -ForegroundColor Yellow
$tessdataPath = "Tests\tessdata"
if (-not (Test-Path $tessdataPath)) {
    New-Item -ItemType Directory -Path $tessdataPath -Force
    Write-Host "Created tessdata directory" -ForegroundColor Green
}

# Download eng.traineddata
$tessdataUrl = "https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata"
$trainedDataPath = Join-Path $tessdataPath "eng.traineddata"

if (-not (Test-Path $trainedDataPath)) {
    Write-Host "Downloading Tesseract language data..." -ForegroundColor Yellow
    try {
        Invoke-WebRequest -Uri $tessdataUrl -OutFile $trainedDataPath
        Write-Host "Tesseract language data downloaded successfully!" -ForegroundColor Green
    } catch {
        Write-Host "Failed to download Tesseract language data: $_" -ForegroundColor Red
    }
} else {
    Write-Host "Tesseract language data already exists" -ForegroundColor Green
}

# Restore and build the solution
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build --configuration Release

# Run database migrations
Update-Database -Project "PaperlessREST"

Write-Host "Build process completed successfully!" -ForegroundColor Green