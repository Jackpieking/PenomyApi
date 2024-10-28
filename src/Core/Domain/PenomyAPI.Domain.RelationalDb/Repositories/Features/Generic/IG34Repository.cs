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

    Task<string> GenerateResetPasswordTokenAsync(
        string tokenId,
        string userId,
        CancellationToken ct
    );

    Task<bool> SavePreResetPasswordTokenMetadataAsync(
        UserToken preResetPasswordToken,
        CancellationToken ct
    );

    Task<long> GetUserIdByEmailAsync(string email, CancellationToken ct);
}
