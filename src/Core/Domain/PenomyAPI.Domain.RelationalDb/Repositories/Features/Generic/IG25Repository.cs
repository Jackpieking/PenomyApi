using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG25Repository
{
    /// <summary>
    ///     Get the total numbers of artwork by the specified
    ///     <paramref name="artworkType"/> and <paramref name="userId"/>.
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
    Task<int> GetTotalOfArtworksByUserIdAndTypeAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    /// <summary>
    ///    Get all the artworks by the specified <paramref name="userId"/>
    ///    and <paramref name="artworkType"/> using pagination.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user wants to view histories.
    /// </param>
    /// <param name="artType">
    ///     The type of the artworks.
    /// </param>
    /// <param name="pageNum">
    ///     The number of current page.
    /// </param>
    /// <param name="artNum">
    ///     The number of artwork to show.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return the user's artworks view history.
    ///     Otherwise, empty.
    /// </returns>
    Task<IEnumerable<IEnumerable<UserArtworkViewHistory>>> GetArtworkViewHistByUserIdAndTypeWithPaginationAsync(
        long userId,
        ArtworkType artType,
        int pageNum,
        int artNum,
        CancellationToken ct
    );

    /// <summary>
    ///    Add the user's artworks view history by the specified <paramref name="userId"/>,
    ///    <paramref name="chapterId"/> and <paramref name="artworkType"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="artworkId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     The type of the artworks.
    /// </param>
    /// <param name="type">
    ///     The type of the artworks.
    /// </param>
    /// <param name="limitChapter">
    ///     The number of chapters can be saved for one art.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if add succesfully.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> AddUserArtworkViewHistAsync(
        long userId,
        long artworkId,
        long chapterId,
        ArtworkType type,
        int limitChapter,
        CancellationToken ct
    );
}
