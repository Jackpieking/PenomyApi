using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG34Repository
{
    /// <summary>
    ///     Validate the password format.
    /// </summary>
    /// <param name="newUser">
    ///     The new user.
    /// </param>
    /// <param name="newPassword">
    ///     The new password.
    /// </param>
    /// <returns>
    ///     True if the password is valid.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> ValidatePasswordAsync(User newUser, string newPassword);

    Task<bool> IsTokenFoundByTokenIdAsync(string tokenId, CancellationToken ct);

    Task<bool> SavePasswordResetTokenMetadatAsync(
        string preResetPasswordTokenId,
        string userId,
        string resetPasswordTokenId,
        CancellationToken ct
    );

    Task<bool> SavePreResetPasswordTokenMetadataAsync(
        UserToken preResetPasswordToken,
        CancellationToken ct
    );

    Task<long> GetUserIdByEmailAsync(string email, CancellationToken ct);

    Task<long> GetResetPasswordTokenInfoByTokenIdAsync(
        string resetPasswordTokenId,
        CancellationToken ct
    );

    Task<bool> UpdatePasswordAsync(
        long userId,
        string newPassword,
        string resetPasswordTokenId,
        CancellationToken ct
    );
}
