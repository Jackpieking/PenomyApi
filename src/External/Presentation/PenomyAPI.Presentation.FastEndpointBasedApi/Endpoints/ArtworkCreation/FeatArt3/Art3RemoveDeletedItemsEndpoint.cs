using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3RemoveDeletedItemsEndpoint
    : Endpoint<Art3RemoveDeletedItemsRequestDto, Art3RemoveDeletedItemsHttpResponse>
{
    public override void Configure()
    {
        Post("art3/deleted/remove");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art3RemoveDeletedItemsRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art3RemoveDeletedItemsRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creator to remove their temporarily items.";
            summary.Description = "This endpoint is used for removing temporarily items of current creator profile.";
            summary.Response<Art3RemoveDeletedItemsHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<Art3RemoveDeletedItemsHttpResponse> ExecuteAsync(
        Art3RemoveDeletedItemsRequestDto requestDto,
        CancellationToken ct)
    {
        if (requestDto.ArtworkIds.Length == 0)
        {
            return Art3RemoveDeletedItemsHttpResponse.CREATOR_HAS_NO_PERMISSION;
        }

        var stateBag = ProcessorState<StateBag>();

        var request = new Art3RemoveDeletedItemsRequest
        {
            CreatorId = stateBag.AppRequest.UserId,
            DeletedArtworkIds = requestDto.ArtworkIds
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art3RemoveDeletedItemsRequest, Art3RemoveDeletedItemsResponse>(request, ct);

        var httpResponse = Art3RemoveDeletedItemsHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
