using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.Chat3;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Chat3;

internal sealed class Chat3Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Chat3Request); // IFeatureRequest của feat

    public override Type FeatHandlerType => typeof(Chat3Handler); // IFeatureHandler của feat

    // Override lại hàm này để đăng ký các dependencies cần thiết cho feat-handler.
    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
