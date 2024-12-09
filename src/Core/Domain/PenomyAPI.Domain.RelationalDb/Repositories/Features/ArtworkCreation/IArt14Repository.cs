using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt14Repository
{
    Task<bool> IsChapterExistedAsync(
        long chapterId,
        CancellationToken cancellationToken);

    Task<bool> IsCurrentCreatorHasPermissionAsync(
        long creatorId,
        long chapterId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Remove the specified chapter from the artwork.
    /// </summary>
    /// <param name="creatorId">
    ///     Id of the creator who executes this action.
    /// </param>
    /// <param name="artworkId">
    ///     Id of the artwork that contains the chapter. Use this artworkId for inner operation.
    /// </param>
    /// <param name="chapterId">
    ///     Id of the chapter to be removed.
    /// </param>
    Task<bool> RemoveArtworkChapterByIdAsync(
        long creatorId,
        long artworkId,
        long chapterId,
        CancellationToken cancellationToken);
}
