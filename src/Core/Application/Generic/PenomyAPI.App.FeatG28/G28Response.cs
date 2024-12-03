using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;

namespace PenomyAPI.App.FeatG28;

public class G28Response : IFeatureResponse
{
    public List<G28ArtworkDetailReadModel> Result { get; set; }

    public G28ResponseStatusCode StatusCode { get; set; }

    public static readonly G28Response DATABASE_ERROR = new()
    {
        StatusCode = G28ResponseStatusCode.DATABASE_ERROR,
    };
}
