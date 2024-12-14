using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using PenowmyAPI.APP.Chat2;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Chat2;

internal sealed class Chat2Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Chat2Request); // IFeatureRequest của feat

    public override Type FeatHandlerType => typeof(Chat2Handler); // IFeatureHandler của feat

    // Override lại hàm này để đăng ký các dependencies cần thiết cho feat-handler.
    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
