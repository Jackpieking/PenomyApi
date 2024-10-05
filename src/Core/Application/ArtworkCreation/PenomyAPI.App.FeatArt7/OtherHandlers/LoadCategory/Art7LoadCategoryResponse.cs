using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;

public class Art7LoadCategoryResponse : IFeatureResponse
{
    public IEnumerable<Category> Categories { get; init; }
}
