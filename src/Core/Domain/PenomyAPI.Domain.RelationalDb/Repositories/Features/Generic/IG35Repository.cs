using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG35Repository
{
    /// <summary>
    ///     Check if the refresh-token id and refresh-token value
    ///     are matched under the database or not.
    /// </summary>
    /// <param name="refreshTokenId"></param>
    /// <param name="refreshTokenValue"></param>
    /// <param name="userId">
    ///     Id of the user that has this refresh-token.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsRefreshTokenValidAsync(
        string refreshTokenId,
        string refreshTokenValue,
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if the current user with specified id
    ///     has registered as a creator or not to fetch the profile.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to check.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsUserRegisteredAsCreatorByIdAsync(
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the profile of the user with specified input id.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to get from.
    /// </param>
    /// <param name="isCreator">
    ///     The flag to indicate to fetch the creator profile of current user.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The <see cref="Task{UserProfile}"/> contains the user profile.
    /// </returns>
    Task<UserProfile> GetUserProfileByIdAsync(
        long userId,
        bool isCreator,
        CancellationToken cancellationToken);
}
