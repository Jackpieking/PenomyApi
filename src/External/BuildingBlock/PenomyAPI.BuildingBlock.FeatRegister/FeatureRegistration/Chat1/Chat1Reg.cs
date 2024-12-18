using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatChat1;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Chat1;

internal sealed class Chat1Reg : FeatureDefinitionRegistration
{
    public override Type FeatRequestType => typeof(Chat1Request); // IFeatureRequest của feat

    public override Type FeatHandlerType => typeof(Chat1Handler); // IFeatureHandler của feat

    // Override lại hàm này để đăng ký các dependencies cần thiết cho feat-handler.
    public override void AddFeatureDependency(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
    }
}
