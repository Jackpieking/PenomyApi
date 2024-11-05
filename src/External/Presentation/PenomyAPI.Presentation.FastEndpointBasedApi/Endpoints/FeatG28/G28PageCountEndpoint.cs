using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG28.PageCount;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.PageCount.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.PageCount.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.PageCount;

public class G28PageCountEndpoint : Endpoint<G28PageCountRequest, G28PageCountHttpResponse>
{
    public override void Configure()
    {
        Get("/g28/user-profile/created-artworks/page-count");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G28PageCountPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting createdartworks for authenticated user.";
            summary.Description =
                "This endpoint is used for getting created artworks for authenticated user.";
            summary.Response<G28PageCountHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G28PageCountResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G28PageCountHttpResponse> ExecuteAsync(
        G28PageCountRequest req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G28PageCountStateBag>();

        req.SetUserId(stateBag.PageCountRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<
            G28PageCountRequest,
            G28PageCountResponse
        >(req, ct);

        var httpResponse = G28PageCountHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        httpResponse.Body = new G28PageCountResponse
        {
            result = featResponse.result,
            StatusCode = featResponse.StatusCode,
        };

        return httpResponse;
    }
}
