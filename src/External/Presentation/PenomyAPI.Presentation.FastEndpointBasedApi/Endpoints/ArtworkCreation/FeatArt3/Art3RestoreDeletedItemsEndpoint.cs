using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3RestoreDeletedItemsEndpoint
    : Endpoint<Art3RestoreDeletedItemsRequestDto, Art3RestoreDeletedItemsHttpResponse>
{
    public override void Configure()
    {
        Post("art3/deleted/restore");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art3RestoreDeletedItemsRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art3RestoreDeletedItemsRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creator to restore their temporarily items.";
            summary.Description = "This endpoint is used for restoring temporarily items of current creator profile.";
            summary.Response<Art3RemoveDeletedItemsHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<Art3RestoreDeletedItemsHttpResponse> ExecuteAsync(
        Art3RestoreDeletedItemsRequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art3RestoreDeletedItemsRequest
        {
            CreatorId = stateBag.AppRequest.UserId,
            ArtworkIds = requestDto.ArtworkIds
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art3RestoreDeletedItemsRequest, Art3RestoreDeletedItemResponse>(request, ct);

        var httpResponse = Art3RestoreDeletedItemsHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
