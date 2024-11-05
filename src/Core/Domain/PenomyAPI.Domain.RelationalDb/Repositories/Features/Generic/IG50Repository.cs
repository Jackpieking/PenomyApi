using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG50Repository
{
    Task<bool> RevokeStarForArtworkAsync(long userId, long artworkId, CancellationToken token = default);
}
