using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1;

public class G1VerifyRegistrationTokenEndpoint : Endpoint<G1VerifyRegistrationTokenRequest>
{
    public override void Configure()
    {
        Get("g1/register/{registrationToken}");

        AllowAnonymous();
    }

    public override async Task<object> ExecuteAsync(G1VerifyRegistrationTokenRequest request, CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenResponse>(
                request, ct);

        return Results.Ok();
    }
}
