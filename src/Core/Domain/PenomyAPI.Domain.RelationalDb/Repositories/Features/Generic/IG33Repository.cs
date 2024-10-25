using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG33Repository
{
    Task<bool> RemoveRefreshTokenAsync(string refreshTokenId, CancellationToken ct);

    Task<bool> IsRefreshTokenFoundByIdAsync(string refreshTokenId, CancellationToken ct);
}
