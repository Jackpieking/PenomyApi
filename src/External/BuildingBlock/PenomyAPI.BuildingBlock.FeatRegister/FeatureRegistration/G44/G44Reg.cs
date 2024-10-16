using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G44;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G44
{
    internal sealed class G43Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G44Request);

        public override Type FeatHandlerType => typeof(G44Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
