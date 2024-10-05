using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration.Common;
using SnowflakeIdGenerator;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration;

internal sealed class SnowflakeIdGeneratorServicesRegistration : IServicesRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<ISnowflakeIdGenerator, AppSnowflakeIdGenerator>()
            .MakeSingletonLazy<ISnowflakeIdGenerator>();
    }
}
