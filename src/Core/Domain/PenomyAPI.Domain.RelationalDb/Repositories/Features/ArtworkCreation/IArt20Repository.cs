using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt20Repository
{
    Task<bool> IsCurrentCreatorAuthorizedToAccessAsync(
        long creatorId,
        long artworkId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the detail of the anime with specified <paramref name="animeId"/>
    ///     to create the new chapter for the anime.
    /// </summary>
    /// <remarks>
    ///     The return detail will include animeId, anime title and last chapter's upload order.
    /// </remarks>
    /// <param name="animeId">
    ///     The id of the anime to get detail support for creating a new chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Artwork"/> instance contains the anime detail.
    /// </returns>
    Task<Artwork> GetDetailToCreateChapterAsync(
        long animeId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Create a new anime chapter with specified detail.
    /// </summary>
    /// <param name="animeChapter">
    ///     The anime chapter instance to create under database.
    /// </param>
    /// <param name="chapterVideoMedia">
    ///     The video media of this anime chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance represent the result of creation.
    /// </returns>
    Task<bool> CreateAnimeChapterAsync(
        ArtworkChapter animeChapter,
        ArtworkChapterMedia chapterVideoMedia,
        CancellationToken cancellationToken
    );
}
