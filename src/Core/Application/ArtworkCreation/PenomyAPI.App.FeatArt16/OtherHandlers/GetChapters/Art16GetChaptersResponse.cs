using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt16.OtherHandlers.GetChapters;

public class Art16GetChaptersResponse : IFeatureResponse
{
    public IEnumerable<ArtworkChapter> Chapters { get; set; }
}
