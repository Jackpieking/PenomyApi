using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG34.OtherHandlers.VerifyResetPasswordToken;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG34;

internal sealed class G34VerifyResetPasswordTokenReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G34VerifyResetPasswordTokenRequest);

    public override Type FeatHandlerType => typeof(G34VerifyResetPasswordTokenHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
