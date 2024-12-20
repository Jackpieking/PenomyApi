using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG9;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG9Repository
{
    Task<ArtworkChapter> GetComicChapterDetailByIdAsync(
        long comicId,
        long chapterId,
        CancellationToken cancellationToken);

    Task<List<G9ChapterItemReadModel>> GetAllChaptersAsyncByComicId(
        long comicId,
        CancellationToken cancellationToken);
}
