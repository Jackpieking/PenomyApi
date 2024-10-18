using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt10Repository
{
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
