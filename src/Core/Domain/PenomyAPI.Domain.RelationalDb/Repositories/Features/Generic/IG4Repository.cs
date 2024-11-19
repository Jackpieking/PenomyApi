using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG4Repository
{
    /// <summary>
    ///     Check if the guest with specified id has viewed any artwork or not.
    /// </summary>
    /// <param name="guestId">
    ///     Id of the guest to check.
    /// </param>
    Task<bool> IsCurrentGuestHasViewHistoryAsync(
        long guestId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the list of recommended artworks for the current user.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to get.
    /// </param>
    /// <returns>
    ///     The list of recommended artworks.
    /// </returns>
    Task<List<RecommendedComicByCategory>> GetRecommendedComicsForUserAsync(
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the list of recommended artworks for the current guest (not signed-in user).
    /// </summary>
    /// <param name="guestId">
    ///     Id of the guest to get.
    /// </param>
    /// <returns>
    ///     The list of recommended artworks.
    /// </returns>
    Task<List<RecommendedComicByCategory>> GetRecommendedComicsForGuestAsync(
        long guestId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the list of recommended artworks for the
    ///     guest that first time visited the platform.
    /// </summary>
    /// <returns>
    ///     The list of recommended artworks.
    /// </returns>
    Task<List<RecommendedComicByCategory>> GetRecommendedComicsForNewGuestAsync(
        CancellationToken cancellationToken);
}
