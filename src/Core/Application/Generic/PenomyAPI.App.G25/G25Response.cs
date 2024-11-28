using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;
using System.Collections.Generic;

namespace PenomyAPI.App.G25;

public class G25Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<G25ViewHistoryArtworkReadModel> Result { get; set; }

    public G25ResponseStatusCode StatusCode { get; set; }

    public static readonly G25Response EMPTY_VIEW_HISTORY = new()
    {
        IsSuccess = true,
        Result = new(),
        StatusCode = G25ResponseStatusCode.SUCCESS
    };
}
