using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PenomyAPI.BuildingBlock.FeatRegister.ServicesRegistration.Common;

/// <summary>
///     The base interface to auto-register the app services using reflection.
/// </summary>
internal interface IServicesRegistration
{
    void Register(IServiceCollection services, IConfiguration configuration);
}
