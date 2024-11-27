using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Base;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Common;

public interface IArtworkChapterRepository
    : IEntityRepository<ArtworkChapter>
{
    /// <summary>
    ///     Check if the artwork chapter with specified <paramref name="chapterId"/>
    ///     is existed in the database or not.
    /// </summary>
    /// <remarks>
    ///     This method only checks the existence of the artwork with the specified id,
    ///     not including any check such as the <see cref="Entities.ArtworkCreation.Common.ArtworkPublicLevel"/>,
    ///     the taken down flag, etc.
    /// </remarks>
    /// <param name="chapterId">
    ///     The id of the chapter to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsChapterExistedByIdAsync(
        long chapterId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if the specified chapter with input id
    ///     is belonged to the specified artwork.
    /// </summary>
    /// <param name="artworkId">
    ///     Id of the artwork to check.
    /// </param>
    /// <param name="chapterId">
    ///     Id of the chapter to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result.
    /// </returns>
    Task<bool> IsChapterBelongedToArtworkByIdAsync(
        long artworkId,
        long chapterId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if the artwork chapter with specified <paramref name="chapterId"/>
    ///     is available to display to the users or not.
    /// </summary>
    /// <remarks>
    ///     This method will check the existence of the chapter with the specified id,
    ///     and including more check to ensure the availability such as: 
    ///     <see cref="Entities.ArtworkCreation.Common.ArtworkPublicLevel"/>,
    ///     the <see cref="ArtworkChapter.PublishStatus"/> and <see cref="ArtworkChapter.IsTemporarilyRemoved"/> flags, etc.
    /// </remarks>
    /// <param name="chapterId">
    ///     The id of the chapter to check.
    /// </param>
    /// <param name="userId">
    ///     The id of the user to check if it can display to this user.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsChapterAvailableToDisplayByIdAsync(
        long chapterId,
        long userId);

    /// <summary>
    ///     Check if the artwork chapter with specified <paramref name="chapterId"/>
    ///     is available to display to the users or not.
    /// </summary>
    /// <remarks>
    ///     This method will check the existence of the chapter with the specified id,
    ///     and including more check to ensure the availability such as: 
    ///     <see cref="Entities.ArtworkCreation.Common.ArtworkPublicLevel"/>,
    ///     the <see cref="ArtworkChapter.PublishStatus"/> and <see cref="ArtworkChapter.IsTemporarilyRemoved"/> flags, etc.
    /// </remarks>
    /// <param name="chapterId">
    ///     The id of the chapter to check.
    /// </param>
    /// <param name="userId">
    ///     The id of the user to check if it can display to this user.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsChapterAvailableToDisplayByIdAsync(
        long artworkId,
        long chapterId,
        long userId);
}
