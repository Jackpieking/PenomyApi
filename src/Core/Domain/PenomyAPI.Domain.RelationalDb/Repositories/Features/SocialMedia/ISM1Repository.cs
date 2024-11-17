using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM1Repository
{
    /// <summary>
    ///     Get user's profile by the specified
    ///     <paramref name="userId"/>.
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
    Task<UserProfile> GetUserFrofileByUserIdAsync(
        long userId,
        CancellationToken cancellationToken);
}
