using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.SaveArtViewHist;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G25Reg
{
    internal sealed class G25SaveArtViewHistReq : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G25SaveArtViewHistRequest);

        public override Type FeatHandlerType => typeof(G25SaveArtViewHistHandle);

        public override void AddFeatureDependency(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
