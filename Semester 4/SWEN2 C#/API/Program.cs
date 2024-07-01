using Autofac;
using Autofac.Extensions.DependencyInjection;
using BL.Module;
using DAL.Module;
using ORM.Module;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
(context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services)
);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => {
    containerBuilder.RegisterModule(new PostgreContextModule(builder.Configuration));
    containerBuilder.RegisterModule(new BusinessLogicModule(builder.Configuration));
    containerBuilder.RegisterModule(new OrmModule());
});

builder.Services.AddCors(options => {
    options.AddPolicy(
    "AllowUI",
    policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:7226")
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowUI");
app.UseStaticFiles();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(
new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/Error", AllowStatusCode404Response = true
}
);
app.MapControllers();

app.Run();
