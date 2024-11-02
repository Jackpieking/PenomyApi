using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatG28.PageCount;

public class G28PageCountResponse : IFeatureResponse
{
    public long result { get; set; }

    public G28PageCountResponseStatusCode StatusCode { get; set; }
}
