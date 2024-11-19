using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public List<RecentlyUpdatedComicReadModel> ArtworkList { get; set; }

    public FeatG3ResponseStatusCode StatusCode { get; set; }
}
