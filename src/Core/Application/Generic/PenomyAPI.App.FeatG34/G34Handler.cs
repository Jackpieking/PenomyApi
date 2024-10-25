using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG34;

public sealed class G34Handler : IFeatureHandler<G34Request, G34Response>
{
    public Task<G34Response> ExecuteAsync(G34Request request, CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}
