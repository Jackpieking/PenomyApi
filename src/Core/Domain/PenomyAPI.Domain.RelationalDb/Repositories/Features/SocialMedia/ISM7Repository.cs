using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia
{
    public interface ISM7Repository
    {
        /// <summary>
        ///     Get the total numbers of joined groups by
        ///     the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">
        ///     The id of the user.
        /// </param>
        /// <param name="ct"></param>
        /// <returns>
        ///     The total numbers of joined groups.
        /// </returns>
        Task<int> GetTotalOfArtworksByTypeAndUserIdAsync(
            long userId,
            CancellationToken ct);

        /// <summary>
        ///     Get all joined groups by the specified
        ///     <paramref name="userId"/> using pagination.
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
        Task<ICollection<SocialGroup>> GetJoinedGroupsByUserIdWithPaginationAsync(
            long userId,
            int pageNum,
            int groupNum,
            CancellationToken ct);
    }
}
