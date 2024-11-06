using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.Common;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG9Repository
{
    Task<ArtworkChapter> GetComicChapterDetailByIdAsync(
        long comicId,
        long chapterId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the prev and next chapter of the chapter
    ///     with specified order in the specified comic.
    /// </summary>
    /// <param name="comicId">
    ///     Id of the comic that contains the chapter.
    /// </param>
    /// <param name="currentChapterOrder">
    ///     Current order of the specified chapter.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PreviousAndNextChapter> GetPreviousAndNextChapterOfCurrentChapterAsync(
        long comicId,
        int currentChapterOrder,
        CancellationToken cancellationToken);
}
