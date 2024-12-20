using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1;
using PenomyAPI.App.FeatArt1.OtherHandlers.OverviewStatistic;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public class Art1OverviewStatisticEndpoint : Endpoint<EmptyDto, Art1OverviewStatisticHttpResponse>
{
    public override void Configure()
    {
        Get("art1/overview-statistic");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<EmptyDto>>();
        PreProcessor<ArtworkCreationPreProcessor<EmptyDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for getting overview statistic of the current creator studio.";
            summary.Description = "This endpoint is used for getting overview statistic of the current creator studio";
            summary.ExampleRequest = new() { };
            summary.Response<Art1HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = $"ART1.{Art1ResponseAppCode.SUCCESS}"
                }
            );
        });
    }

    public override async Task<Art1OverviewStatisticHttpResponse> ExecuteAsync(
        EmptyDto empty,
        CancellationToken ct)
    {
        // Get the state bag contains userId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = new Art1OverviewStatisticRequest
        {
            CreatorId = creatorId
        };

        var featureResponse = await FeatureExtensions.ExecuteAsync<Art1OverviewStatisticRequest, Art1OverviewStatisticResponse>(
            request,
            ct);

        var httpResponse = new Art1OverviewStatisticHttpResponse
        {
            Body = featureResponse
        };

        await SendAsync(httpResponse, StatusCodes.Status200OK, ct);

        return httpResponse;
    }
}
