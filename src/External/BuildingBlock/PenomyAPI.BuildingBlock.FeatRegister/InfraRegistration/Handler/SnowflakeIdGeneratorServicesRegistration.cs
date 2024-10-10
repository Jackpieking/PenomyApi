using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using SnowflakeIdGenerator;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

internal sealed class SnowflakeIdGeneratorServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<ISnowflakeIdGenerator, AppSnowflakeIdGenerator>()
            .MakeSingletonLazy<ISnowflakeIdGenerator>();
    }
}
