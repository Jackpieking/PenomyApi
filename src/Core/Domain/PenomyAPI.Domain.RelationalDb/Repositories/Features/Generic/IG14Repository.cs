using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG14Repository
{
    Task<List<Artwork>> GetRecommendedAnimeAsync(long cateId, int limit = 5, CancellationToken cancellationToken = default);
    Task<bool> IsExistCategoryAsync(long cateId, CancellationToken token = default);
}
