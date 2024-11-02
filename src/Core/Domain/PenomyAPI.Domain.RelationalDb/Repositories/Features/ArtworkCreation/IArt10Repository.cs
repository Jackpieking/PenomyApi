using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt10Repository
{
    /// <summary>
    ///     Get the detail of the comic with specified <paramref name="comicId"/>
    ///     to create the new chapter for the comic.
    /// </summary>
    /// <remarks>
    ///     The return detail will include comicId, comic title and last chapter's upload order.
    /// </remarks>
    /// <param name="comicId">
    ///     The id of the comic to get detail support for creating a new chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Artwork"/> instance contains the comic detail.
    /// </returns>
    Task<Artwork> GetDetailToCreateChapterByComicIdAsync(
        long comicId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Create a new comic chapter with specified detail.
    /// </summary>
    /// <param name="comicChapter">
    ///     The comic chapter instance to create under database.
    /// </param>
    /// <param name="chapterMedias">
    ///     The belonging medias of this comic chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{Boolean}"/> instance represent the result of creation.
    /// </returns>
    Task<bool> CreateComicChapterAsync(
        ArtworkChapter comicChapter,
        IEnumerable<ArtworkChapterMedia> chapterMedias,
        CancellationToken cancellationToken
    );
}
