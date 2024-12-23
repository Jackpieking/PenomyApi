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
        /// <param name="artworkType">
        ///     The artwork's type.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     True if data has been committed.
        ///     Otherwise, false.
        /// </returns>
        Task<bool> FollowArtwork(long userId, long artworkId, ArtworkType artworkType, CancellationToken ct);

        /// <summary>
        ///     Check user has followed the artwork.
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
        ///     True if user has followed.
        ///     Otherwise, false.
        /// </returns>
        Task<bool> CheckFollowedArtwork(long userId, long artworkId, CancellationToken ct);

        /// <summary>
        ///     Get artwork's type by the specified <paramref name="artworkId"/>.
        /// </summary>
        /// <param name="artworkId">
        ///     The artwork's ID has been followed.
        /// </param>
        /// <param name="ct">
        ///     The token to notify the server to cancel the operation.
        /// </param>
        /// <returns>
        ///     Return artworkType if artwork exist.
        ///     Otherwise, return null.
        /// </returns>
        Task<ArtworkType> GetArtworTypeById(long artworkId, CancellationToken ct);
    }
}
