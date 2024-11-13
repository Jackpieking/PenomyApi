using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;

/// <summary>
///     The read model contains the list of top recommended artworks.
/// </summary>
public sealed class G2TopRecommendedArtworks
{
    public ICollection<Artwork> RecommendedArtworks { get; set; }

    public IList<ArtworkChapter> LatestChapterOfEachArtworks { get; set; }
}
