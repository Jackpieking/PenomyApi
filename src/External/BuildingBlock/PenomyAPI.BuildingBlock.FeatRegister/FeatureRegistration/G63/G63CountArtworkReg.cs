using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenomyAPI.App.G63.OtherHandlers.CountArtwork;
using PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.G63
{
    internal sealed class G63CountArtworkReg : FeatureDefinitionRegistration
    {
        public override Type FeatRequestType => typeof(G63CountArtworkRequest);

        public override Type FeatHandlerType => typeof(G63CountArtworkHandler);

        public override void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        )
        { }
    }
}
