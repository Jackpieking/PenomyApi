using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt1;

public class FeatArt1Handler : IFeatureHandler<FeatArt1Request, FeatArt1Response>
{
    public Task<FeatArt1Response> ExecuteAsync(FeatArt1Request request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
