using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG37Repository
{
    /// <summary>
    ///     Check if the user with specified id
    ///     has already registered as a creator or not.
    /// </summary>
    /// <param name="userId">
    ///     The userId to check.
    /// </param>
    Task<bool> HasUserAlreadyBecomeCreatorAsync(
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Register the current user as a creator on the platform.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to register.
    /// </param>
    Task<bool> RegisterUserAsCreatorAsync(
        long userId,
        CancellationToken cancellationToken);
}
