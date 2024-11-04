using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG32Repository
{
    Task<bool> IsUserFoundByEmailAsync(string email, CancellationToken ct);

    Task<bool> AddNewUserAndRefreshTokenToDatabaseAsync(
        User user,
        UserProfile userProfile,
        UserToken refreshToken,
        CancellationToken ct
    );

    Task<bool> CreateRefreshTokenCommandAsync(UserToken refreshToken, CancellationToken ct);

    Task<long> GetCurrentUserIdAsync(string email, CancellationToken ct);
}
