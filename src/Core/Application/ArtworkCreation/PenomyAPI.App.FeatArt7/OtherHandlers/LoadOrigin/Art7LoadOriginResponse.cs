using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadOrigin;

public sealed class Art7LoadOriginResponse : IFeatureResponse
{
    public IEnumerable<ArtworkOrigin> Origins { get; init; }
}
