using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG48Repository
{
    /// <summary>
    ///     Get the user's favourited artworks.
    /// </summary>
    /// <param name="userId">
    ///     The user's ID.
    /// </param>
    /// <param name="artworkType">
    ///     The artwork's type.
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
    ///     The user's favourited artworks.
    ///     Otherwise, empty.
    /// </returns>
    Task<ICollection<Artwork>> GetAllFavoriteArtworks(
        long userId,
        ArtworkType artworkType,
        CancellationToken ct,
        int pageNum = 1,
        int artNum = 20);
}
