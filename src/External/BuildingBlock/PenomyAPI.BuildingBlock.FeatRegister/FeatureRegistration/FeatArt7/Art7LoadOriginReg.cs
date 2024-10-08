using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt7.OtherHandlers.LoadOrigin;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt7;

internal sealed class Art7LoadOriginReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art7LoadOriginRequest);

    public override Type FeatHandlerType => typeof(Art7LoadOriginHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
