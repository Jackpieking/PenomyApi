using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common
{
    internal interface IFeatureDefinitionRegistration
    {
        public Type FeatRequestType { get; }

        public Type FeatHandlerType { get; }

        public FeatureHandlerDefinition HandlerDefinition { get; }

        void AddFeatureDependency(IServiceCollection services, IConfiguration configuration);
    }
}
