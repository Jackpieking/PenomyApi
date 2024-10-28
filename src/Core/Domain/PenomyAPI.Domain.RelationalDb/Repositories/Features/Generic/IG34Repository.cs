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

    /// <summary>
    ///     Check if the user is found by email.
    /// </summary>
    /// <param name="email">
    ///     The email.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     True if the user is found.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> IsUserFoundByEmailAsync(string email, CancellationToken ct);

    Task<string> GenerateResetPasswordTokenAsync(string email, CancellationToken ct);
}
