using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using Quartz;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

public class BackgroundJobServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartzHostedService();
    }
}
