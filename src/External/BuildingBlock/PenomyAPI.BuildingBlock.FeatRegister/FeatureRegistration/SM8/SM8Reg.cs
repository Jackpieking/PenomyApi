using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM8;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM8
{
    internal sealed class SM8Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM8Request);

        public override Type FeatHandlerType => typeof(SM8Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
