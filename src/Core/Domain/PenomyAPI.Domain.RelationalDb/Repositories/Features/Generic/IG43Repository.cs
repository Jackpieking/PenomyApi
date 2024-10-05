﻿using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic
{
    public interface IG43Repository
    {
        /// <summary>
        ///     Save user data has followed the artwork.
        /// </summary>
        /// <param name="userId">
        ///     The user's ID follows the artwork.
        /// </param>
        /// <param name="artworkId">
        ///     The artwork's ID has been followed.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     True if data has been committed.
        ///     Otherwise, false.
        /// </returns>
        Task<bool> FollowArtwork(long userId, long artworkId, ArtworkType artworkType, CancellationToken ct);
    }
}
