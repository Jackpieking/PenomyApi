using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;

public class Art4LoadCategoryRequest : IFeatureRequest<Art4LoadCategoryResponse>
{
    private Art4LoadCategoryRequest() { }

    public static Art4LoadCategoryRequest Empty { get; } = new Art4LoadCategoryRequest();
}
