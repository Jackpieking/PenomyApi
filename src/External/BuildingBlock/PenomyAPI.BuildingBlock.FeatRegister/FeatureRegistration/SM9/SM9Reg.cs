using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM9;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM9
{
    internal sealed class SM9Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM9Request);

        public override Type FeatHandlerType => typeof(SM9Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
