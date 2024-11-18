using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM1;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM1
{
    internal sealed class SM1Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM1Request);

        public override Type FeatHandlerType => typeof(SM1Handler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
