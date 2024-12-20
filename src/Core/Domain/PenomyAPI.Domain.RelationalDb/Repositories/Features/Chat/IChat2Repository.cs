using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;

public interface IChat2Repository
{
    Task<UserProfile> GetUserProfileByIdAsync(long userId);

    Task<List<ChatGroup>> GetChatGroupsAsync(long userId, CancellationToken token);

    /// <summary>
    ///     Get all the user's group id by the specified <paramref name="userId"/>
    ///     and <paramref name="artworkType"/> using pagination.
    /// </summary>
    /// <remarks>
    ///     This method uses offset-based pagination to retrieve data.
    /// </remarks>
    /// <param name="userId">
    ///     The user's ID.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     The user's favourited artworks.
    ///     Otherwise, empty.
    /// </returns>
    Task<ICollection<long>> GetAllJoinedChatGroupIdAsync(
        long userId,
        CancellationToken token
    );
}
