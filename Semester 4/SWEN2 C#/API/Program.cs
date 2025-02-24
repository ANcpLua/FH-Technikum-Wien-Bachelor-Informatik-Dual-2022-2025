using Autofac;
using Autofac.Extensions.DependencyInjection;
using BL.Module;
using DAL.Infrastructure;
using DAL.Module;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services)
);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new PostgreContextModule(builder.Configuration));
    containerBuilder.RegisterModule(new BusinessLogicModule(builder.Configuration));
    containerBuilder.RegisterModule(new OrmModule());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
        policy
            .WithOrigins(
                "http://localhost:7226",
                "http://localhost",
                "http://tourplanner-ui",
                "http://tourplanner-ui:80"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true)
    );
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
var app = builder.Build();
await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<TourPlannerContext>().Database.EnsureCreatedAsync();
app.UseRouting();
app.UseCors("AllowUI");
app.UseStaticFiles();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = "/Error",
    AllowStatusCode404Response = true
});
app.Run();