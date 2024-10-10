using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Common.FileServices;
using PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.FileService.CloudinaryService;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration;

internal sealed class DefaultDistributedFileServiceRegistration : IServiceRegistration
{
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IDefaultDistributedFileService, CloudinaryFileService>()
            .MakeScopedLazy<IDefaultDistributedFileService>();
    }
}
