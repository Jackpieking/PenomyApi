using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PenomyAPI.BuildingBlock.FeatRegister.FeatureRegistration.Common
{
    internal abstract class FeatureDefinitionRegistration : IFeatureDefinitionRegistration
    {
        private readonly FeatureHandlerDefinition _handlerDefinition;

        public FeatureDefinitionRegistration()
        {
            _handlerDefinition = new(FeatHandlerType);
        }

        public abstract Type FeatRequestType { get; }

        public abstract Type FeatHandlerType { get; }

        public FeatureHandlerDefinition HandlerDefinition => _handlerDefinition;

        public abstract void AddFeatureDependency(
            IServiceCollection services,
            IConfiguration configuration
        );
    }
}
