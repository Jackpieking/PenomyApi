using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default);

    Task<G5ComicDetailReadModel> GetArtWorkDetailByIdAsync(
        long artworkId,
        CancellationToken ct = default
    );

    Task<bool> IsComicInUserFavoriteListAsync(
        long userId,
        long artworkId,
        CancellationToken ct = default
    );

    Task<bool> IsComicInUserFollowedListAsync(
        long userId,
        long artworkId,
        CancellationToken ct = default
    );

    /// <summary>
    ///     Get the first chapter id and last recent
    ///     read chapter id for the current user.
    /// </summary>
    /// <param name="comicId">
    ///     Id of the comic to get from.
    /// </param>
    /// <param name="guestId">
    ///     Id of the guest to get.
    /// </param>
    Task<G5FirstAndLastReadChapterReadModel> GetFirstAndLastReadChapterOfComicForGuestAsync(
        long comicId,
        long guestId,
        CancellationToken ct
    );

    /// <summary>
    ///     Get the first chapter id and last recent
    ///     read chapter id for the current guest.
    /// </summary>
    /// <param name="comicId">
    ///     Id of the comic to get from.
    /// </param>
    /// <param name="userId">
    ///     Id of the user to get.
    /// </param>
    Task<G5FirstAndLastReadChapterReadModel> GetFirstAndLastReadChapterOfComicForUserAsync(
        long comicId,
        long userId,
        CancellationToken ct
    );
}
