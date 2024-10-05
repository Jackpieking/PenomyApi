using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.FeatG4;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.FeatG4
{
    internal sealed class G54Reg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G4Request);

        public override Type FeatHandlerType => typeof(G4Handler);

        public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
