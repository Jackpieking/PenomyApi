using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM5;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM5
{
    internal sealed class SM5Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM5Request);

        public override Type FeatHandlerType => typeof(SM5Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
