using Autofac;
using BL.Mapper;
using Mapster;
using MapsterMapper;

namespace BL.Module;

public class OrmModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var config = MappingConfiguration.RegisterMapping();
        config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
        builder.RegisterInstance(config).As<TypeAdapterConfig>().SingleInstance();
        builder.RegisterType<MapsterMapper.Mapper>().As<IMapper>().InstancePerLifetimeScope();
    }
}