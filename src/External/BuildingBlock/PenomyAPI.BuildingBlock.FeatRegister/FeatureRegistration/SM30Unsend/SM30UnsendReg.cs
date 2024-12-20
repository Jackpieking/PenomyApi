using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM30.SM30UnsendHandler;
using PenomyAPI.App.SM30Unsend.SM30UnsendUnsendHandler;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM30Unsend;

internal sealed class SM30Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(SM30UnsendRequest);

    public override Type FeatHandlerType => typeof(SM30UnsendHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
