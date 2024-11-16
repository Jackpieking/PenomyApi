using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic
{
    public interface IG44Repository
    {

        /// <summary>
        ///     Check if the user with specified id has followed the artwork or not.
        /// </summary>
        /// <param name="userId">
        ///     Id of the user to check.
        /// </param>
        /// <param name="artworkId">
        ///     Id of the artwork to check.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        ///     The boolean result of checking.
        /// </returns>
        Task<bool> HasFollowedArtworkAsync(
            long userId,
            long artworkId,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Delete user data has unfollowed the artwork.
        /// </summary>
        /// <param name="userId">
        ///     The user's ID unfollows the artwork.
        /// </param>
        /// <param name="artworkId">
        ///     The artwork's ID has been unfollowed.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     True if data has been deleted.
        ///     Otherwise, false.
        /// </returns>
        Task<bool> UnFollowArtwork(long userId, long artworkId, CancellationToken ct);
    }
}
