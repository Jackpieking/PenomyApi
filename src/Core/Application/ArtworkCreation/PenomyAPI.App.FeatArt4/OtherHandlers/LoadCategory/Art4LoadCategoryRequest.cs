using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt4.OtherHandlers.LoadCategory;

public class Art4LoadCategoryRequest : IFeatureRequest<Art4LoadCategoryResponse>
{
    private static readonly Art4LoadCategoryRequest _instance = new();

    private Art4LoadCategoryRequest() { }

    public static Art4LoadCategoryRequest Empty => _instance;
}
