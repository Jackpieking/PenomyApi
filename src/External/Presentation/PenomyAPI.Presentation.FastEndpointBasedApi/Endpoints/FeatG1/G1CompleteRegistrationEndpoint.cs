using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1;

public sealed class G1CompleteRegistrationEndpoint : Endpoint<G1CompleteRegistrationRequest>
{
    public override void Configure()
    {
        Post("g1/confirm/register");

        AllowAnonymous();
    }

    public override async Task<object> ExecuteAsync(
        G1CompleteRegistrationRequest req,
        CancellationToken ct)
    {
        var featResponse = await FeatureExtensions
            .ExecuteAsync<G1CompleteRegistrationRequest, G1CompleteRegistrationResponse>(req, ct);

        if (featResponse.StatusCode == G1CompleteRegistrationResponseStatusCode.SUCCESS)
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }
}
