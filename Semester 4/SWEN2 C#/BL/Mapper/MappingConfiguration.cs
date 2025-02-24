using BL.DomainModel;
using DAL.PersistenceModel;
using Mapster;
using UI.Model;

namespace BL.Mapper;

public static class MappingConfiguration
{
    public static TypeAdapterConfig RegisterMapping()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        ConfigureTourMappings(config);
        ConfigureTourLogMappings(config);

        return config;
    }

    private static void ConfigureTourMappings(TypeAdapterConfig config)
    {
        config
            .NewConfig<TourPersistence, TourDomain>()
            .Map(dest => dest.Logs, src => src.TourLogPersistence);

        config
            .NewConfig<TourDomain, TourPersistence>()
            .Map(dest => dest.TourLogPersistence, src => src.Logs);

        config.NewConfig<TourDomain, Tour>().Map(dest => dest.TourLogs, src => src.Logs);

        config.NewConfig<Tour, TourDomain>().Map(dest => dest.Logs, src => src.TourLogs);
    }

    private static void ConfigureTourLogMappings(TypeAdapterConfig config)
    {
        config
            .NewConfig<TourLogDomain, TourLogPersistence>()
            .Map(dest => dest.TourPersistenceId, src => src.TourDomainId);

        config
            .NewConfig<TourLogPersistence, TourLogDomain>()
            .Map(dest => dest.TourDomainId, src => src.TourPersistenceId);

        config
            .NewConfig<TourLogDomain, TourLog>()
            .Map(dest => dest.TourId, src => src.TourDomainId);

        config
            .NewConfig<TourLog, TourLogDomain>()
            .Map(dest => dest.TourDomainId, src => src.TourId);
    }
}