using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Realtime.SignalR;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Handler;

public class SignalRServicesRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<INotificationHub, NotificationHub>()
            .MakeSingletonLazy<INotificationHub>();
    }
}
