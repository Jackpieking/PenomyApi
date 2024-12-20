using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt6;

public sealed class Art6Response : IFeatureResponse
{
    public IEnumerable<ArtworkChapter> Chapters { get; set; }
}
