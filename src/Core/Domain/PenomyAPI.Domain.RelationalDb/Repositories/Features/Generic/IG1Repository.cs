using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG1Repository
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
    Task<bool> IsUserFoundByEmailQueryAsync(string email, CancellationToken ct);

    /// <summary>
    ///     Add new user to database.
    /// </summary>
    /// <param name="newUser">
    ///     The new user.
    /// </param>
    /// <param name="newUserProfile">
    ///     Profile of the new user.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Mail confirmation token if the user is added.
    ///     Otherwise, empty string.
    /// </returns>
    Task<bool> AddNewUserToDatabaseAsync(
        User newUser,
        UserProfile newUserProfile,
        string password,
        CancellationToken ct
    );
}
