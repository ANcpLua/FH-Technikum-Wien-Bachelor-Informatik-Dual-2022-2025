using Contract.Logger;
using PaperlessREST;
using PaperlessServices.AutoMapper;
using PaperlessServices.BL;
using PaperlessServices.Extensions;
using PaperlessServices.MinIoStorage;
using PostgreSQL.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("rest-appsettings.json", optional: false)
    .AddJsonFile($"rest-appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()  
    .Build();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<LoggerFilterOptions>(options =>
{
    options.AddFilter("Microsoft", LogLevel.Warning);
    options.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
    options.AddFilter("System", LogLevel.Warning);
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

builder.Services.AddFluentValidationRules();
builder.Services.AddPaperlessLogger<MinioStorageService>("Storage", environment, ServiceLifetime.Singleton);
builder.Services.AddPaperlessLogger<DocumentService>("Documents", environment, ServiceLifetime.Singleton);
builder.Services.AddPostgreSqlServices(builder.Configuration);
builder.Services.AddElasticSearchEngine(builder.Configuration);
builder.Services.AddMinioObjectStorage(builder.Configuration, builder.Environment);
builder.Services.AddRabbitMqMessageBus(builder.Configuration, builder.Environment);
builder.Services.AddSingleton<IMinioStorageService, MinioStorageService>(); 
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", c => c.
    AllowAnyOrigin().
    AllowAnyMethod().
    AllowAnyHeader());
});
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperConfig>());
builder.WebHost.UseKestrel(options => { options.ListenAnyIP(80); });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

await app.RunAsync();