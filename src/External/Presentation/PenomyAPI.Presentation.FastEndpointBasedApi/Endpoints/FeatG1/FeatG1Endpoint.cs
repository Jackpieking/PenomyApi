using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1;

public class FeatG1Endpoint : Endpoint<FeatArt1Req, FeatArt1Response>
{
    public override void Configure()
    {
        base.Configure();
    }

    public override Task<string> ExecuteAsync(string req, CancellationToken ct)
    {
        return base.ExecuteAsync(req, ct);
    }
}
