using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG25Repository
{
    /// <summary>
    ///     Get the number of artworks the user has viewed.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user wants to view histories.
    /// </param>
    /// <param name="artType">
    ///     The type of the artworks.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return number of artworks the user has viewed if the user has login.
    /// </returns>
    Task<int> ArtworkHistoriesCount(long userId, ArtworkType artType, CancellationToken ct);

    /// <summary>
    ///     Get user artworks view history.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user wants to view histories.
    /// </param>
    /// <param name="artType">
    ///     The type of the artworks.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <param name="pageNum">
    ///     The number of current page.
    /// </param>
    /// <param name="artNum">
    ///     The number of artwork to show.
    /// </param>
    /// <returns>
    ///     Return artworks view history if the user has login.
    ///     Otherwise, false.
    /// </returns>
    Task<IEnumerable<IEnumerable<UserArtworkViewHistory>>> GetArtworkViewHistories(long userId, ArtworkType artType, CancellationToken ct, int pageNum = 1, int artNum = 20);

    /// <summary>
    ///     Get user artworks view history.
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
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <param name="limitChapter">
    ///     The number of chapters can be saved for one art.
    /// </param>
    /// <returns>
    ///     Return artworks view history if the user has login.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> AddArtworkViewHist(long userId, long artworkId, long chapterId, ArtworkType type, CancellationToken ct, int limitChapter = 5);
}
