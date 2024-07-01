using BlazorDownloadFile;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using UI;
using UI.Decorator;
using UI.Service;
using UI.Service.Interface;
using UI.ViewModel;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configuration = builder.Configuration;

builder.Services.AddSingleton(
new HttpClient
{
    BaseAddress = new Uri(configuration["AppSettings:ApiBaseUrl"] ?? "https://localhost:7102")
}
);

builder.Services.AddBlazoredToast();
builder.Services.AddBlazorDownloadFile();
builder.Services.AddSingleton<IConfiguration>(_ => configuration);
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IToastServiceWrapper, ToastService>();
builder.Services.AddScoped<IRouteApiService, RouteApiService>();
builder.Services.AddScoped<IViewModelHelperService, ViewModelHelperService>();
builder.Services.AddScoped<TryCatchToastWrapper>();

builder.Services.AddScoped<TourViewModel>();
builder.Services.AddScoped<TourLogViewModel>();
builder.Services.AddScoped<SearchViewModel>();
builder.Services.AddScoped<MapViewModel>();
builder.Services.AddScoped<ReportViewModel>();

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", "UI")
    .Enrich.FromLogContext()
    .WriteTo.BrowserConsole()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddSingleton(Log.Logger);

await builder.Build().RunAsync();
