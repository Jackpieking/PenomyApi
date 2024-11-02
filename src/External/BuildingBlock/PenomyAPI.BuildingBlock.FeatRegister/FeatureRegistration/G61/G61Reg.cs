using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G61;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G61
{
    internal sealed class G61Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G61Request);

        public override Type FeatHandlerType => typeof(G61Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
