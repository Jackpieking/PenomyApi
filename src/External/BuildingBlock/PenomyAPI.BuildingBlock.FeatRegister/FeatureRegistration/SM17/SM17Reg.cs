using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM17;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM17
{
    internal sealed class SM17Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM17Request);

        public override Type FeatHandlerType => typeof(SM17Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
