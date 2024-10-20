using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG45Repository
{
    /// <summary>
    ///     Get artworks user data has followed .
    /// </summary>
    /// <param name="userId">
    ///     The user's ID follows the artwork.
    /// </param>
    /// <param name="artworkType">
    ///     The artwork's type want to get.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <param name="pageNum">
    ///     The number of current page.
    /// </param>
    /// <param name="artNum">
    ///     The number of artwork to show.
    /// </param>
    /// <returns>
    ///     True if data has been committed.
    ///     Otherwise, false.
    /// </returns>
    Task<ICollection<Artwork>> GetAllFollowedArtworks(
        long userId,
        ArtworkType artworkType,
        CancellationToken ct,
        int pageNum = 1,
        int artNum = 20);
}
