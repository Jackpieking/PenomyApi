using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG45Repository
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
    /// <param name="userId">
    ///     The user's ID follows the artwork.
    /// </param>
    /// <param name="artworkType">
    ///     The artwork's type want to get.
    /// </param>
    /// <param name="pageNum">
    ///     The number of current page.
    /// </param>
    /// <param name="artNum">
    ///     The number of artwork to show.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     True if data has been committed.
    ///     Otherwise, false.
    /// </returns>
    Task<List<G45FollowedArtworkReadModel>> GetFollowedArtworksByTypeAndUserIdWithPaginationAsync(
        long userId,
        ArtworkType artworkType,
        int pageNum,
        int artNum,
        CancellationToken ct
        );
}
