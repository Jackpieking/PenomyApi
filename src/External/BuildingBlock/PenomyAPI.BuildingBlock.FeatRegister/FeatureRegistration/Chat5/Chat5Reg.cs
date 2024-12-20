using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Chat5;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Chat5;

internal sealed class Chat5Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Chat5Request); // IFeatureRequest của feat

    public override Type FeatHandlerType => typeof(Chat5Handler); // IFeatureHandler của feat

    // Override lại hàm này để đăng ký các dependencies cần thiết cho feat-handler.
    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
