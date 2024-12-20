using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG14;
using PenomyAPI.App.FeatG14.OtherHandler;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G14;

internal sealed class G14GuestReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G14GuestRequest);

    public override Type FeatHandlerType => typeof(G14GuestHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    { }
}
