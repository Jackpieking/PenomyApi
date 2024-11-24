using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;

public interface ISM6Repository
{
    /// <summary>
    ///     Check group exist by the specified
    ///     <paramref name="groupId"/>.
    /// </summary>
    /// <param name="groupId">
    ///     The id of the group.
    /// </param>
    /// <param name="ct"></param>
    /// <returns>
    ///     Return true if the group exists.
    ///     Otherwise, return false.
    /// </returns>
    Task<bool> CheckGroupExists(
        long groupId,
        CancellationToken ct);

    /// <summary>
    ///     Check user joined group by the specified
    ///     <paramref name="userId"/> and <paramref name="groupId"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user.
    /// </param>
    /// <param name="groupId">
    ///     The id of the group.
    /// </param>
    /// <param name="ct"></param>
    /// <returns>
    ///     Return true if user joined.
    ///     Otherwise, return false.
    /// </returns>
    Task<bool> CheckUserJoinedGroupAsync(
        long userId,
        long groupId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Add user's join group request by the specified
    ///     <paramref name="userId"/> and <paramref name="groupId"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user.
    /// </param>
    /// <param name="groupId">
    ///     The id of the group.
    /// </param>
    /// <param name="ct"></param>
    /// <returns>
    ///     Return true if add success.
    ///     Otherwise, return false.
    /// </returns>
    Task<bool> AddUserJoinRequestByUserIdAndGroupIdAsync(
        long userId,
        long groupId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Add user to group by the specified
    ///     <paramref name="userId"/> and <paramref name="groupId"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user.
    /// </param>
    /// <param name="groupId">
    ///     The id of the group.
    /// </param>
    /// <param name="ct"></param>
    /// <returns>
    ///     Return true if add success.
    ///     Otherwise, return false.
    /// </returns>
    Task<bool> AddUserToGroupByUserIdAndGroupIdAsync(
        long userId,
        long groupId,
        CancellationToken cancellationToken);
}
