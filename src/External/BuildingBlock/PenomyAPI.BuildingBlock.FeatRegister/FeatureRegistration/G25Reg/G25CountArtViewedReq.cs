using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25.OtherHandlers.NumberArtViewed;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25Reg
{
    internal sealed class G25CountArtViewedReq : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G25CountArtViewedRequest);

        public override Type FeatHandlerType => typeof(G25CountArtViewedHandler);

        public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
