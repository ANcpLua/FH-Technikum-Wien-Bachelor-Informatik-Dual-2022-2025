using Autofac;
using DAL.Infrastructure;
using DAL.Interface;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Module;

public class PostgreContextModule : Autofac.Module
{
    private readonly IConfiguration _configuration;

    public PostgreContextModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(_ => {
                var connectionString = _configuration.GetConnectionString(
                "TourPlannerDatabase"
                );
                var dbOptions = new DbContextOptionsBuilder<TourPlannerContext>()
                    .UseNpgsql(connectionString)
                    .Options;
                return new TourPlannerContext(dbOptions);
            })
            .InstancePerLifetimeScope();

        builder.RegisterType<TourRepository>().As<ITourRepository>().InstancePerLifetimeScope();
        builder
            .RegisterType<TourLogRepository>()
            .As<ITourLogRepository>()
            .InstancePerLifetimeScope();
    }
}
