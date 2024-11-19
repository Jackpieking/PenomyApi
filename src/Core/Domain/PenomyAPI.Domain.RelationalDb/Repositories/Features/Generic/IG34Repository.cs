using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG34Repository
{
    Task<bool> ValidatePasswordAsync(User newUser, string newPassword);

    Task<bool> IsTokenFoundByTokenIdAsync(string tokenId, CancellationToken ct);

    Task<bool> SaveResetPasswordTokenMetadataAsync(
        UserToken preResetPasswordToken,
        CancellationToken ct
    );

    Task<long> GetUserIdByEmailAsync(string email, CancellationToken ct);

    Task<string> GetUserEmailByUserIdAsync(long userId, CancellationToken ct);

    Task<bool> UpdatePasswordAsync(
        long userId,
        string newPassword,
        string resetPasswordTokenId,
        CancellationToken ct
    );
}
