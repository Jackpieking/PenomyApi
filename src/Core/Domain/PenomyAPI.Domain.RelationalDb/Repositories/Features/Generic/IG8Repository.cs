using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG8Repository
{
    Task<List<ArtworkChapter>> GetChapterByComicIdWithPaginationAsync(
        long id,
        int startPage = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );

    Task<int> GetTotalChaptersByComicIdAsync(
        long comicId,
        CancellationToken cancellationToken);

    Task<bool> IsArtworkExistAsync(
        long id,
        CancellationToken token = default);
}
