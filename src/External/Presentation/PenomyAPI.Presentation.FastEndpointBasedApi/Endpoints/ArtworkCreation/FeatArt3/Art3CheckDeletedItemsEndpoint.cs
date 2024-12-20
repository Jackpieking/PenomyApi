using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.CheckDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3CheckDeletedItemsEndpoint
    : Endpoint<EmptyRequest, Art3CheckDeletedItemsHttpResponse>
{
    public override void Configure()
    {
        Get("art3/deleted/check");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<EmptyRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<EmptyRequest>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary =
                "Endpoint for checking if the current creator account has any deleted artworks.";
            summary.Description =
                "This endpoint is used for checking if the current creator account has any deleted artworks.";
            summary.Response<Art3HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { HttpCode = StatusCodes.Status200OK, }
            );
        });
    }

    public override async Task<Art3CheckDeletedItemsHttpResponse> ExecuteAsync(
        EmptyRequest req,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art3CheckDeletedItemsRequest { CreatorId = stateBag.AppRequest.UserId };

        var featureResponse = await FeatureExtensions.ExecuteAsync<
            Art3CheckDeletedItemsRequest,
            Art3CheckDeletedItemsResponse
        >(request, ct);

        var httpResponse = Art3CheckDeletedItemsHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
