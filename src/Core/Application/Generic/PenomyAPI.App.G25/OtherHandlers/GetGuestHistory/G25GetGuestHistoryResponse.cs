using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;
using System.Collections.Generic;

namespace PenomyAPI.App.G25.OtherHandlers.GetGuestHistory;

public sealed class G25GetGuestHistoryResponse : IFeatureResponse
{
    public G25GetGuestHistoryResponseAppCode AppCode { get; set; }

    /// <summary>
    ///     The list contains all artworks that current guest has viewed.
    /// </summary>
    public List<G25ViewHistoryArtworkReadModel> ViewedArtworks { get; set; }

    public static G25GetGuestHistoryResponse SUCCESS(List<G25ViewHistoryArtworkReadModel> viewedArtworks) => new()
    {
        AppCode = G25GetGuestHistoryResponseAppCode.SUCCESS,
        ViewedArtworks = viewedArtworks
    };

    public static G25GetGuestHistoryResponse EMPTY_VIEW_HISTORY = new()
    {
        AppCode = G25GetGuestHistoryResponseAppCode.EMPTY_VIEW_HISTORY,
    };

    public static readonly G25GetGuestHistoryResponse GUEST_ID_NOT_FOUND = new()
    {
        AppCode = G25GetGuestHistoryResponseAppCode.GUEST_ID_NOT_FOUND,
    };
}
