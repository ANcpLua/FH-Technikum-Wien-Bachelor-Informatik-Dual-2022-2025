using Autofac;
using BL.Interface;
using BL.Service;
using Microsoft.Extensions.Configuration;

namespace BL.Module;

public class BusinessLogicModule : Autofac.Module
{
    private readonly IConfiguration _configuration;

    public BusinessLogicModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TourService>().As<ITourService>().InstancePerLifetimeScope();
        builder.RegisterType<TourLogService>().As<ITourLogService>().InstancePerLifetimeScope();
        builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();

        builder
            .RegisterType<PdfReportService>()
            .As<IPdfReportService>()
            .WithParameter(
            "imageBasePath",
            _configuration["AppSettings:ImageBasePath"] ?? string.Empty
            )
            .InstancePerLifetimeScope();
    }
}