using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Contract.Logger;

public static class LoggerExtensions
{
    public static void AddPaperlessLogger<T>(this IServiceCollection services,
        string component,
        string environment,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(
            typeof(IPaperlessLogger),
            sp => new PaperlessLogger(
                sp.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(T).Name),
                component,
                environment),
            lifetime));
    }
}