using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG5Repository
{
    Task<bool> IsArtworkExistAsync(long artworkId, CancellationToken ct = default);

    Task<G5ComicDetailReadModel> GetArtWorkDetailByIdAsync(long artworkId, CancellationToken ct = default);

    Task<bool> IsComicInUserFavoriteListAsync(long userId, long artworkId, CancellationToken ct = default);

    Task<bool> IsComicInUserFollowedListAsync(long userId, long artworkId, CancellationToken ct = default);
}
