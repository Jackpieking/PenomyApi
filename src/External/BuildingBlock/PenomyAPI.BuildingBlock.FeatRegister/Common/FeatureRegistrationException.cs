using System;

namespace PenomyAPI.BuildingBlock.FeatRegister.Common
{
    public sealed class FeatureRegistrationException : Exception
    {
        public FeatureRegistrationException(string message)
            : base(message) { }
    }
}
