using Autofac;
using Mapster;
using MapsterMapper;

namespace ORM.Module;

public class OrmModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var config = MappingConfiguration.RegisterMapping();
        config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
        builder.RegisterInstance(config).As<TypeAdapterConfig>().SingleInstance();
        builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
    }
}
