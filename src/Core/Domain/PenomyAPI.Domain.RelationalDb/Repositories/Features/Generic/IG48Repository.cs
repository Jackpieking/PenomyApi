using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG48Repository
{
    /// <summary>
    ///     Get the total numbers of artwork by the specified
    ///     <paramref name="artworkType"/> and <paramref name="userId"/>.
    /// </summary>
    /// <param name="artworkType">
    ///     The type of artwork to take.
    /// </param>
    /// <param name="userId">
    ///     The id of the user who favorite these artworks.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The total numbers of artwork.
    /// </returns>
    Task<int> GetTotalOfArtworksByTypeAndUserIdAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get all the artworks by the specified <paramref name="userId"/>
    ///     and <paramref name="artworkType"/> using pagination.
    /// </summary>
    /// <remarks>
    ///     This method uses offset-based pagination to retrieve data.
    /// </remarks>
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
    Task<List<G48FavoriteArtworkReadModel>> GetFavoriteArtworksByTypeAndUserIdWithPaginationAsync(
        long userId,
        ArtworkType artworkType,
        int pageNum,
        int artNum,
        CancellationToken ct);
}
