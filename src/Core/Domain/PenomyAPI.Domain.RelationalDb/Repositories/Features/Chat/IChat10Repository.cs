using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat
{
    public interface IChat10Repository
    {
        /// <summary>
        ///     Check group exist by the specified
        ///     <paramref name="chatGroupId"/>.
        /// </summary>
        /// <remarks>
        ///     This method uses offset-based pagination to retrieve data.
        /// </remarks>
        /// <param name="groupId">
        ///     The group's ID.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     Return true, if group exist.
        ///     Otherwise, false.
        /// </returns>
        Task<bool> CheckGroupExistAsync(
            long chatGroupId,
            CancellationToken ct = default
        );

        /// <summary>
        ///     Get reply message Id by the specified
        ///     <paramref name="chatId"/>.
        /// </summary>
        /// <remarks>
        ///     This method uses offset-based pagination to retrieve data.
        /// </remarks>
        /// <param name="chatId">
        ///     The chat's ID.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     Return reply Id, if exist.
        ///     Otherwise, empty.
        /// </returns>
        Task<long> GetMessageReplyByChatIdAsync(
            long chatId,
            CancellationToken ct = default
        );

        /// <summary>
        ///     Get chat from group by the specified
        ///     <paramref name="chatGroupId"/>.
        /// </summary>
        /// <remarks>
        ///     This method uses offset-based pagination to retrieve data.
        /// </remarks>
        /// <param name="groupId">
        ///     The group's ID.
        /// </param>
        /// <param name="pageNum">
        ///     The number of current page.
        /// </param>
        /// <param name="chatNum">
        ///     The number of chat to show.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     The group's chat content.
        ///     Otherwise, empty.
        /// </returns>
        Task<ICollection<ChatMessage>> GetChatGroupByGroupIdAsync(
            long chatGroupId,
            int pageNum,
            int chatNum,
            CancellationToken ct = default
        );
    }
}
