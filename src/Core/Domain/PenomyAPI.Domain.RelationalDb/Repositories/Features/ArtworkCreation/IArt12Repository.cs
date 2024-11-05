using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt12Repository
{
    /// <summary>
    ///     Check if the current chapter is temporarily removed
    ///     or not by the specified input id.
    /// </summary>
    /// <param name="chapterId">
    ///     Id of the chapter to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsChapterTemporarilyRemovedByIdAsync(
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Check if the chapter with input id and comicId is existed or not.
    /// </summary>
    /// <param name="comicId">
    ///     Id of the comic that contains the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     Id of the chapter to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsComicChapterExistedAsync(
        long comicId,
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Check if the input the creator with specified input id has permission
    ///     to access and update the specified chapter or not.
    /// </summary>
    /// <param name="creatorId">
    ///     Id of the creator that wants to update the chapter detail.
    /// </param>
    /// <param name="chapterId">
    ///     Id of the chapter that will be updated.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Return <see langword="true"/> if the creator has permission
    ///     to update the specified chapter. Otherwise, <see langword="false"/>.
    /// </returns>
    Task<bool> HasPermissionToUpdateChapterDetailAsync(
        long creatorId,
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Get the detail of the chapter by the specified input id.
    /// </summary>
    /// <param name="chapterId">
    ///     Id of the chapter to get the detail.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     An <see cref="ArtworkChapter"/> instance that contains detail of the chapter.
    /// </returns>
    Task<ArtworkChapter> GetChapterDetailByIdAsync(
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Get the current upload order of the specified chapter.
    /// </summary>
    /// <param name="chapterId">
    ///     Id of the chapter to get upload order.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCurrentUploadOrderByChapterIdAsync(
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Update the comic chapter detail by the provided information.
    /// </summary>
    /// <param name="changeFromDrafted">
    ///     Specify to update the chapter from drafted mode to other mode.
    ///     If true, then must have updated in the artwork upload order.
    /// </param>
    /// <param name="chapterDetail">
    ///     The detail of the chapter to update.
    /// </param>
    /// <param name="updatedChapterMediaItems">
    ///     The list of chapter media items to update.
    /// </param>
    /// <param name="deletedChapterMediaIds">
    ///     The list of chapter media items to delete.
    /// </param>
    /// <param name="createdNewChapterMediaItems">
    ///     The list of chapter media items to create new.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contain updating result.
    /// </returns>
    Task<bool> UpdateComicChapterAsync(
        bool changeFromDrafted,
        ArtworkChapter chapterDetail,
        IEnumerable<ArtworkChapterMedia> updatedChapterMediaItems,
        IEnumerable<long> deletedChapterMediaIds,
        IEnumerable<ArtworkChapterMedia> createdNewChapterMediaItems,
        CancellationToken cancellationToken
    );

    Task<List<ArtworkChapterMedia>> GetChapterMediasByChapterIdAsync(
        long chapterId,
        CancellationToken cancellationToken
    );
}
