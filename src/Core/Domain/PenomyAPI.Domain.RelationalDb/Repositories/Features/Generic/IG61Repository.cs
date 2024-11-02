using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG61Repository
{
    /// <summary>
    ///     Allow the user to follow a creator.
    /// </summary>
    /// <param name="userId">
    ///     The user's ID.
    /// </param>
    /// <param name="creatorId">
    ///     The creator's ID.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if the user success follows a creator.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> FollowCreator(
        long userId,
        long creatorId,
        CancellationToken ct);

    /// <summary>
    ///     Check the user has followed a creator.
    /// </summary>
    /// <param name="creatorId">
    ///     The creator's ID.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if the user already followed a creator.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> IsCreator(
        long creatorId,
        CancellationToken ct);

    /// <summary>
    ///     Check the user has followed a creator.
    /// </summary>
    /// <param name="userId">
    ///     The user's ID.
    /// </param>
    /// <param name="creatorId">
    ///     The creator's ID.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if the user already followed a creator.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> IsFollowedCreator(
        long userId,
        long creatorId,
        CancellationToken ct);
}
