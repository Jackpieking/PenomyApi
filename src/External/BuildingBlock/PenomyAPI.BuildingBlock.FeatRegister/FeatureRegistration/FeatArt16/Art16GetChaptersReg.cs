using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatArt16.OtherHandlers.GetChapters;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatArt16;

internal class Art16GetChaptersReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Art16GetChaptersRequest);

    public override Type FeatHandlerType => typeof(Art16GetChaptersHandler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
