using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostgreSQL.Data;
using PostgreSQL.Repository;

namespace PostgreSQL.Module;

public static class DatabaseExtension
{
    public static void AddPostgreSqlServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<PaperlessDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsAssembly("PostgreSQL"));
                
            options.UseLoggerFactory(LoggerFactory.Create(builder => 
                builder.AddFilter((category, level) => 
                    category == DbLoggerCategory.Database.Command.Name && 
                    level == LogLevel.Warning)));
                    
            options.EnableSensitiveDataLogging(false);
        });

        services.AddScoped<IDocumentRepository, DocumentRepository>();
    }
}