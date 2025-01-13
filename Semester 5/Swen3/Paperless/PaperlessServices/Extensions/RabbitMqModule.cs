using EasyNetQ;

namespace PaperlessServices.Extensions;

public static class RabbitMqModule
{
    public static void AddRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddSingleton<IBus>(_ =>
        {
            var env = environment.EnvironmentName;
            var host = env == "Docker" ? "rabbitmq" : "localhost";
            var port = configuration["RabbitMQ:Port"] ?? "5672";
            var username = configuration["RabbitMQ:Username"] ?? "guest";
            var password = configuration["RabbitMQ:Password"] ?? "guest";
            
            var connectionString = $"host={host};port={port};username={username};password={password}";
            return RabbitHutch.CreateBus(connectionString);
        });
    }
}