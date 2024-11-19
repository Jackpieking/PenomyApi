using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG3;
using PenomyAPI.App.FeatG3.Infrastructures;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.Infra.FeatG3;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG3;

internal sealed class FeatG3Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(FeatG3Request);

    public override Type FeatHandlerType => typeof(FeatG3Handler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IFeatG3MailHandler, FeatG3MailHandler>();
    }
}
