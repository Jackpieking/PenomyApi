using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G63;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G63
{
    internal sealed class G63Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G63Request);

        public override Type FeatHandlerType => typeof(G63Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
