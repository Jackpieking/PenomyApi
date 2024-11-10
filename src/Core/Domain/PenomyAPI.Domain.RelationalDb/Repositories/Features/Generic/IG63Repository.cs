using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG63Repository
{
    /// <summary>
    ///     Get the total numbers of artwork by the specified
    ///     <paramref name="artworkType"/> and <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user who favorite these artworks.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The total numbers of artwork.
    /// </returns>
    Task<int> GetTotalOfCreatorByUserIdAsync(
        long userId,
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
    Task<ICollection<CreatorProfile>> GetFollowedCreatorsByUserIdWithPaginationAsync(
        long userId,
        int pageNum,
        int creatorNum,
        CancellationToken ct);
}
