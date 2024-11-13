using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG2Repository
{
    /// <summary>
    ///     Get the list of the top recommended artworks
    ///     based on the input artwork type.
    /// </summary>
    /// <param name="artworkType">
    ///     The type of the artwork to filter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The list contains the top recommended artworks based on type.
    /// </returns>
    Task<G2TopRecommendedArtworks> GetTopRecommendedArtworksByTypeAsync(
        ArtworkType artworkType,
        CancellationToken cancellationToken);
}
