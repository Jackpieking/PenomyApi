using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G43;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G43
{
    internal sealed class G43Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G43Request);

        public override Type FeatHandlerType => typeof(G43Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
