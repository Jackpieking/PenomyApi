using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG8Repository
{
    Task<List<ArtworkChapter>> GetArtWorkChapterByIdAsync(
        long id,
        int startPage = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );
    Task<ArtworkChapterMetaData> GetArtworkChapterMetaDataAsync(
        long id,
        CancellationToken token = default
    );
}
