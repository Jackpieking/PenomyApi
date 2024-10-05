using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration.Common;
using PenomyAPI.FileService.CloudinaryService;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration;

internal sealed class DefaultDistributedFileServiceRegistration
    : IServicesRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IDefaultDistributedFileService, CloudinaryFileService>()
            .MakeScopedLazy<IDefaultDistributedFileService>();
    }
}
