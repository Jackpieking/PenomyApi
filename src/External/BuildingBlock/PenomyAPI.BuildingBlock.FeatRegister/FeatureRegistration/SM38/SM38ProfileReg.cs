using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM38.GroupProfile;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM38
{
    internal sealed class SM38ProfileReg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM38ProfileRequest);

        public override Type FeatHandlerType => typeof(SM38ProfileHandler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
