using Contract.Logger;
using PaperlessServices.BL;
using PaperlessServices.Extensions;
using PaperlessServices.MinIoStorage;
using PaperlessServices.TesseractOCR;
using PostgreSQL.Module;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("service-appsettings.json", optional: false)
    .AddJsonFile($"service-appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()  
    .Build();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddFluentValidationRules();
builder.Services.AddControllers();
builder.Services.AddPostgreSqlServices(builder.Configuration);
builder.Services.AddMinioObjectStorage(builder.Configuration, builder.Environment);
builder.Services.AddRabbitMqMessageBus(builder.Configuration, builder.Environment);
builder.Services.AddElasticSearchEngine(builder.Configuration);
builder.Services.AddDocumentProcessing();
builder.Services.AddTesseractOcr();
builder.Services.AddAutoMapperProfiles();

builder.Services.AddPaperlessLogger<MinioStorageService>("Storage", builder.Environment.EnvironmentName, ServiceLifetime.Singleton);
builder.Services.AddPaperlessLogger<OcrWorkerService>("Worker", builder.Environment.EnvironmentName, ServiceLifetime.Singleton);
builder.Services.AddPaperlessLogger<Ocr>("OCR", builder.Environment.EnvironmentName, ServiceLifetime.Singleton); 
builder.Services.AddPaperlessLogger<DocumentService>("Documents", builder.Environment.EnvironmentName, ServiceLifetime.Singleton);

var app = builder.Build();

await app.RunAsync();