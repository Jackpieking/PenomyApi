using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Base;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.UnitOfWorks.Common.Common;

/// <summary>
///     The repository for the <see cref="Artwork"/> entity, includes common methods to check the <see cref="Artwork"/>.
/// </summary>
public interface IArtworkRepository : IEntityRepository<Artwork>
{
    /// <summary>
    ///     Check if the artwork with specified <paramref name="artworkId"/>
    ///     is existed in the database or not.
    /// </summary>
    /// <remarks>
    ///     This method is only check the existence of the artwork with the specified id,
    ///     not including any check such as the <see cref="Entities.ArtworkCreation.Common.ArtworkPublicLevel"/>,
    ///     the taken down flag, etc.
    /// </remarks>
    /// <param name="artworkId">
    ///     The id of the artwork to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsArtworkExistedByIdAsync(
        long artworkId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if the artwork with specified <paramref name="artworkId"/>
    ///     is available to display to the users.
    /// </summary>
    /// <remarks>
    ///     This method will check the existence of the artwork with the specified id,
    ///     and including more check to ensure the availability such as: 
    ///     <see cref="Entities.ArtworkCreation.Common.ArtworkPublicLevel"/>,
    ///     the <see cref="Artwork.IsTakenDown"/> and <see cref="Artwork.IsTemporarilyRemoved"/> flags, etc.
    /// </remarks>
    /// <param name="artworkId">
    ///     The id of the artwork to check.
    /// </param>
    /// <param name="userId">
    ///     The id of the user to check if it can display to this user.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance contains the result of checking.
    /// </returns>
    Task<bool> IsArtworkAvailableToDisplayByIdAsync(
        long artworkId,
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the chapter thumbnail default image url
    ///     based on the input <paramref name="artworkId"/>.
    /// </summary>
    /// <remarks>
    ///     This method is supported when create a new chapter
    ///     of an artwork without uploading the thumbnail image file for that chapter.
    /// </remarks>
    /// <param name="artworkId">
    ///     The artworkId to get the thumbnail url.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> GetChapterThumbnailDefaultUrlByArtworkIdAsync(
        long artworkId,
        CancellationToken cancellationToken);
}
