using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG35;

internal sealed class G35GetCreatorProfileReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G35GetCreatorProfileRequest);

    public override Type FeatHandlerType => typeof(G35GetCreatorProfileHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }
}
