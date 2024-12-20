using System.Collections.Generic;
using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;

public class Art7LoadCategoryResponse : IFeatureResponse
{
    public IEnumerable<Category> Categories { get; init; }
}
