using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG3;

public class FeatG3Request : IFeatureRequest<FeatG3Response>
{
    private FeatG3Request() { }

    public static FeatG3Request Empty { get; } = new FeatG3Request();
}
