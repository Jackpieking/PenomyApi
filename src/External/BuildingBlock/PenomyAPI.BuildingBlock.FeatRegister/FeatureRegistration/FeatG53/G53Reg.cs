using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG53;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG53
{
    internal sealed class G53Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G53Request);

        public override Type FeatHandlerType => typeof(G53Handler);

        public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
