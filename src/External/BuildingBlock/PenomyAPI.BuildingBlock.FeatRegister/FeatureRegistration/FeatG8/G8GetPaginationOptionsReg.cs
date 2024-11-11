using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG8.OtherHandlers;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG8;

internal sealed class G8GetPaginationOptionsReg
    : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G8GetPaginationOptionsRequest);

    public override Type FeatHandlerType => typeof(G8GetPaginationOptionsHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
