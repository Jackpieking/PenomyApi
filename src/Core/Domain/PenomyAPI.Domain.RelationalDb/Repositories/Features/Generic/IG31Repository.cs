using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG31Repository
{
    Task<bool> IsUserFoundByEmailAsync(string email, CancellationToken ct);

    Task<(
        bool isPasswordCorrect,
        bool isUserTemporarilyLockedOut,
        long userIdOfUserHasBeenValidated
    )> CheckPasswordSignInAsync(string email, string password, CancellationToken ct);

    Task<bool> CreateRefreshTokenCommandAsync(UserToken refreshToken, CancellationToken ct);

    Task<UserProfile> GetUserProfileAsync(long userId, CancellationToken ct);
}
