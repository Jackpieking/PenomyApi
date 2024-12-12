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
    ///     Check if the user with specified id is existed or not.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to check.
    /// </param>
    Task<bool> IsUserIdExistedAsync(
        long userId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if the creator with specified id is existed or not.
    /// </summary>
    /// <param name="creatorId">
    ///     Id of the creator to check.
    /// </param>
    Task<bool> IsCreatorIdExistedAsync(
        long creatorId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the profile of the user with specified input id.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to get from.
    /// </param>
    /// <param name="isProfileOwner">
    ///     This flag to indicate the request is served for
    ///     the owner of this user profile.
    ///     The repository will depend on this value to fetch the data properly.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{UserProfile}"/> contains the user profile.
    /// </returns>
    Task<UserProfile> GetUserProfileByIdAsync(
        long userId,
        bool isProfileOwner,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the profile of the user that has already registered
    ///     as a creator with specified input id.
    /// </summary>
    /// <param name="userId">
    ///     Id of the user to get from.
    /// </param>
    /// <param name="isProfileOwner">
    ///     This flag to indicate the request is served for
    ///     the owner of this user profile.
    ///     The repository will depend on this value to fetch the data properly.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{UserProfile}"/> contains the user profile.
    /// </returns>
    Task<UserProfile> GetUserProfileAsCreatorByIdAsync(
        long userId,
        bool isProfileOwner,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the profile of the creator with specified input id.
    /// </summary>
    /// <param name="creatorId">
    ///     Id of the user to get from.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{UserProfile}"/> contains the creator profile.
    /// </returns>
    Task<UserProfile> GetCreatorProfileByIdAsync(
        long creatorId,
        CancellationToken cancellationToken);
}
