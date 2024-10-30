using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG31ARepository
{
    Task<bool> UpdateRefreshTokenAsync(string oldTokenId, string newTokenId, CancellationToken ct);

    Task<bool> IsRefreshTokenFoundAsync(
        string refreshTokenId,
        string refreshTokenValue,
        CancellationToken ct
    );

    Task<bool> IsRefreshTokenExpiredAsync(string refreshTokenId, CancellationToken ct);
}
