using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG34.OtherHandlers.CompleteResetPassword;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG34;

internal sealed class G34CompleteResetPasswordReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G34CompleteResetPasswordRequest);

    public override Type FeatHandlerType => typeof(G34CompleteResetPasswordHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    ) { }
}
