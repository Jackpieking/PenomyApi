using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.SM38.CoverImage;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.SM38
{
    internal sealed class SM38ImageReg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(SM38ImageRequest);

        public override Type FeatHandlerType => typeof(SM38ImageHandler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        ) { }
    }
}
