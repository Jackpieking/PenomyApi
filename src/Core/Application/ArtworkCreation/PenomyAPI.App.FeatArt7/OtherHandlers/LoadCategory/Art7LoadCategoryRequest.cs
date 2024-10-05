using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadCategory;

public class Art7LoadCategoryRequest : IFeatureRequest<Art7LoadCategoryResponse>
{
    private static readonly Art7LoadCategoryRequest _instance = new();

    private Art7LoadCategoryRequest() { }

    public static Art7LoadCategoryRequest Empty => _instance;
}
