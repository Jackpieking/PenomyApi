using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PenomyAPI.BuildingBlock.FeatRegister.InfraRegistration.Common;

/// <summary>
///     The base interface to auto-register the app services using reflection.
/// </summary>
internal interface IServiceRegistration
{
    void Register(IServiceCollection services, IConfiguration configuration);
}
