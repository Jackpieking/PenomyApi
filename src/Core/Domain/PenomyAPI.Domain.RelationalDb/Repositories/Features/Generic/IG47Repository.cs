using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG47Repository
{
    Task<bool> RemoveFromFavoriteAsync(long userId, long artworkId, CancellationToken token = default);
}
