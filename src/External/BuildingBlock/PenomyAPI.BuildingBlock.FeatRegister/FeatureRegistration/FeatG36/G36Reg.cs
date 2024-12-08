using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG36;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG36;

internal sealed class G36Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G36Request);

    public override Type FeatHandlerType => typeof(G36Handler);

    public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
    {
    }
}
