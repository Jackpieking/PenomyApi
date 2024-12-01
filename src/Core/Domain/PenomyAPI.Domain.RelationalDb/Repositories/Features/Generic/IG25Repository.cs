using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;

public interface IG25Repository
{
    Task<bool> IsUserViewHistoryNotEmptyAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the total numbers of artwork by the specified
    ///     <paramref name="artworkType"/> and <paramref name="userId"/>.
    /// </summary>
    /// <param name="artworkType">
    ///     The type of artwork to take.
    /// </param>
    /// <param name="userId">
    ///     The id of the user who favorite these artworks.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     The total numbers of artwork.
    /// </returns>
    Task<int> GetTotalOfArtworksByUserIdAndTypeAsync(
        long userId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    /// <summary>
    ///    Get all the artworks by the specified <paramref name="userId"/>
    ///    and <paramref name="artType"/> using pagination.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user wants to view histories.
    /// </param>
    /// <param name="artType">
    ///     The type of the artworks.
    /// </param>
    /// <param name="pageNum">
    ///     The number of current page.
    /// </param>
    /// <param name="artNum">
    ///     The number of artwork to show.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return the user's artworks view history.
    ///     Otherwise, empty.
    /// </returns>
    Task<List<G25ViewHistoryArtworkReadModel>> GetArtworkViewHistByUserIdAndTypeWithPaginationAsync(
        long userId,
        ArtworkType artType,
        int pageNum,
        int artNum,
        CancellationToken ct
    );

    /// <summary>
    ///     Check if current user has viewed the this artwork or not.
    /// </summary>
    /// <param name="userId">
    ///     Id of the guest to check
    /// </param>
    /// <param name="artworkId">
    ///     Id of the artwork to check.
    /// </param>
    Task<bool> IsUserViewHistoryRecordExistedAsync(
        long userId,
        long artworkId,
        CancellationToken cancellationToken);

    /// <summary>
    ///    Add the user's artworks view history by the specified <paramref name="userId"/>,
    ///    <paramref name="chapterId"/> and <paramref name="artworkType"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="artworkId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     The type of the artworks.
    /// </param>
    /// <param name="artworkType">
    ///     The type of the artworks.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if add succesfully.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> AddUserViewHistoryAsync(
        long userId,
        long artworkId,
        long chapterId,
        ArtworkType artworkType,
        CancellationToken ct
    );

    /// <summary>
    ///    Update the user's artworks view history by the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="artworkId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     The type of the artworks.
    /// </param>
    /// <param name="ct">
    ///     The token to notify the server to cancel the operation.
    /// </param>
    /// <returns>
    ///     Return true if add succesfully.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> UpdateUserViewHistoryAsync(
        long userId,
        long artworkId,
        long chapterId,
        CancellationToken ct
    );

    Task<ArtworkType> GetArtworkTypeForAddingViewHistoryRecordAsync(
        long artworkId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Init a new guest view history with specified id and begin track the guest activity.
    /// </summary>
    /// <param name="guestId">
    ///     Id of the guest to track the view history.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> InitGuestViewHistoryAsync(
        long guestId,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Get the guest tracking by the specified <paramref name="guestId"/>.
    /// </summary>
    /// <param name="guestId">
    ///     Id of the guest to get the tracking record.
    /// </param>
    Task<GuestTracking> GetGuestTrackingByIdAsync(
        long guestId,
        CancellationToken cancellationToken);

    Task<bool> IsGuestIdExistedAsync(
        long guestId,
        CancellationToken cancellationToken);

    Task<bool> IsGuestViewHistoryNotEmptyAsync(
        long guestId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    Task<List<G25ViewHistoryArtworkReadModel>> GetGuestViewHistoryByArtworkTypeAsync(
        long guestId,
        ArtworkType artworkType,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Check if current guest has viewed the this artwork or not.
    /// </summary>
    /// <param name="userId">
    ///     Id of the guest to check
    /// </param>
    /// <param name="artworkId">
    ///     Id of the artwork to check.
    /// </param>
    Task<bool> IsGuestViewHistoryRecordExistedAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken);

    /// <summary>
    ///    Add the guest's artworks view history by the specified <paramref name="guestId"/>,
    ///    <paramref name="chapterId"/> and <paramref name="artworkType"/>.
    /// </summary>
    /// <param name="guestId">
    ///     The id of the guest has viewed the chapter.
    /// </param>
    /// <param name="artworkId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     The type of the artworks.
    /// </param>
    /// <param name="artworkType">
    ///     The type of the artworks.
    /// </param>
    /// <returns>
    ///     Return true if add succesfully.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> AddGuestViewHistoryAsync(
        long guestId,
        ArtworkType artworkType,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken);

    /// <summary>
    ///    Update the guest's artworks view history by the specified <paramref name="guestId"/>,
    ///    <paramref name="chapterId"/> .
    /// </summary>
    /// <param name="guestId">
    ///     The id of the guest has viewed the chapter.
    /// </param>
    /// <param name="artworkId">
    ///     The id of the user has viewed the chapter.
    /// </param>
    /// <param name="chapterId">
    ///     The type of the artworks.
    /// </param>
    /// <returns>
    ///     Return true if add succesfully.
    ///     Otherwise, false.
    /// </returns>
    Task<bool> UpdateGuestViewHistoryAsync(
        long guestId,
        long artworkId,
        long chapterId,
        DateTime viewedAt,
        CancellationToken cancellationToken);

    Task<bool> RemoveGuestViewHistoryItemAsync(
        long guestId,
        long artworkId,
        CancellationToken cancellationToken);

    Task<bool> RemoveUserViewHistoryItemAsync(
       long userId,
       long artworkId,
       CancellationToken cancellationToken);
}
