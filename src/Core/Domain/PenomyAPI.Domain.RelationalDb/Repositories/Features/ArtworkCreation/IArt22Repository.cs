using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt22Repository
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
    ///     Get the current publish status of the specified chapter.
    /// </summary>
    /// <param name="chapterId">
    ///     Id of the chapter to get publish status.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     <see cref="Task{PublishStatus}"/> contains the result of current chapter publish status.
    /// </returns>
    Task<PublishStatus> GetCurrentChapterPublishStatusAsync(
        long chapterId,
        CancellationToken cancellationToken
    );

    /// <summary>
    ///     Update the comic chapter detail by the provided information.
    /// </summary>
    /// <param name="isChangedFromDraftedToOtherPublishStatus">
    ///     Specify to update the chapter from drafted mode to other mode.
    ///     If true, then must have updated in the artwork upload order.
    /// </param>
    /// <param name="isScheduleDateTimeChanged">
    ///     If <see langword="false"/>, then update the content of the chapter only
    ///     without affecting its publish-status and published at.
    ///     <br/> 
    ///     If <see langword="true"/>, update the content of the chapter
    ///     with its publish-status and published at.
    /// </param>
    /// <param name="chapterDetail">
    ///     The detail of the chapter to update.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contain updating result.
    /// </returns>
    Task<bool> UpdateAnimeChapterAsync(
        bool isChangedFromDraftedToOtherPublishStatus,
        bool isScheduleDateTimeChanged,
        ArtworkChapter chapterDetail,
        ArtworkChapterMedia chapterVideoMedia,
        CancellationToken cancellationToken
    );
}
