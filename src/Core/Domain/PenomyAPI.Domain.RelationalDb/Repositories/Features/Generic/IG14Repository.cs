using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG14Repository
{
    Task<List<Artwork>> GetRecommendedAnimeAsync(long cateId, CancellationToken cancellationToken = default);
    Task<bool> IsExistCategoryAsync(long cateId, CancellationToken token = default);
    Task<List<Category>> GetUserFavoritesCategoryIdsAsync(long userId, int totalCategoriesToTake = 4, CancellationToken token = default);
    Task<List<long>> GetCategoryIdsFromGuestViewHistoryAsync(long guestId, int totalToTake = 4, CancellationToken token = default);
}
