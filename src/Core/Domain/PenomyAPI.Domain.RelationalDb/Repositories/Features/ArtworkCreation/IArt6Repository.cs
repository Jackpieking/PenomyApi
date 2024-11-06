using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.ArtworkCreation;

public interface IArt6Repository
{
    Task<IEnumerable<ArtworkChapter>> GetChaptersByPublishStatusAsync(
        long comicId,
        PublishStatus publishStatus,
        CancellationToken cancellationToken);
}
