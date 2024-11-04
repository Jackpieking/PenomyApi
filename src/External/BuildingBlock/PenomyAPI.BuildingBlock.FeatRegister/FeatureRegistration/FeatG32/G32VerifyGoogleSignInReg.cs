using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG32.Infrastructures;
using PenomyAPI.App.FeatG32.OtherHandlers.VerifyGoogleSignIn;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenomyAPI.BuildingBlock.FeatRegister.ServiceExtensions;
using PenomyAPI.Infra.FeatG32;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG32;

internal sealed class G32VerifyGoogleSignInReg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(G32VerifyGoogleSignInRequest);

    public override Type FeatHandlerType => typeof(G32VerifyGoogleSignInHandler);

    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddSingleton<
                IG32GetUserProfileAvatarUrlFromGoogleHandler,
                G32GetUserProfileAvatarUrlFromGoogleHandler
            >()
            .MakeSingletonLazy<IG32GetUserProfileAvatarUrlFromGoogleHandler>();
    }
}
