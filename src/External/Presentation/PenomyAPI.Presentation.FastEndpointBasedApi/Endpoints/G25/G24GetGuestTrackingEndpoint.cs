using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.GetGuestTracking;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25GetGuestTrackingEndpoint
    : Endpoint<G25GetGuestTrackingRequest, G25GetGuestTrackingHttpResponse>
{
    public override void Configure()
    {
        Get("g25/guest");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status404NotFound);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get guest tracking detail.";
            summary.Description = "This endpoint is used for get guest tracking detail.";
            summary.Response<G25GetGuestTrackingHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25GetGuestTrackingResponseAppCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G25GetGuestTrackingHttpResponse> ExecuteAsync(
        G25GetGuestTrackingRequest request,
        CancellationToken ct
    )
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<
            G25GetGuestTrackingRequest,
            G25GetGuestTrackingResponse
        >(request, ct);

        var httpResponse = G25GetGuestTrackingHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
