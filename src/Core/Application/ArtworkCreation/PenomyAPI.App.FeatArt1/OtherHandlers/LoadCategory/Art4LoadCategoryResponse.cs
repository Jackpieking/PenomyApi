using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;

public class Art4LoadCategoryResponse : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public Art4LoadCategoryResponseStatusCode StatusCode { get; set; }

    public IEnumerable<Category> Categories { get; set; }
}
