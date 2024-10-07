using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG54;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG54
{
    internal sealed class G54Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G54Request);

        public override Type FeatHandlerType => typeof(G54Handler);

        public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
