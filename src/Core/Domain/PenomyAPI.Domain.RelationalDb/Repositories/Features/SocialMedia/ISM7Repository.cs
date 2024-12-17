using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia
{
    public interface ISM7Repository
    {
        /// <summary>
        ///     Get all joined groups by the specified
        ///     <paramref name="userId"/>.
        /// </summary>
        /// <remarks>
        ///     This method uses offset-based pagination to retrieve data.
        /// </remarks>
        /// <param name="userId">
        ///     The user's ID.
        /// </param>
        /// <param name="pageNum">
        ///     The number of current page.
        /// </param>
        /// <param name="groupNum">
        ///     The number of group to show.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     The user's joined groups.
        ///     Otherwise, empty.
        /// </returns>
        Task<ICollection<SocialGroup>> GetJoinedGroupsByUserIdAsync(
            long userId,
            int pageNum,
            int groupNum,
            CancellationToken ct = default
        );

        /// <summary>
        ///     Get all unjoined groups by the specified
        ///     <paramref name="userId"/>.
        /// </summary>
        /// <remarks>
        ///     This method uses offset-based pagination to retrieve data.
        /// </remarks>
        /// <param name="userId">
        ///     The user's ID.
        /// </param>
        /// <param name="pageNum">
        ///     The number of current page.
        /// </param>
        /// <param name="groupNum">
        ///     The number of group to show.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     The user's unjoined groups.
        ///     Otherwise, empty.
        /// </returns>
        Task<ICollection<SocialGroup>> GetUnjoinedGroupsByUserIdAsync(
            long userId,
            int pageNum,
            int groupNum,
            CancellationToken ct = default
        );
    }
}
