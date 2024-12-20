using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RemoveAllDeteledItems;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3RemoveAllDeletedItemsEndpoint
    : Endpoint<Art3RestoreOrRemoveItemRequestDto, Art3RemoveAllDeletedItemsHttpResponse>
{
    public override void Configure()
    {
        Post("art3/deleted/artworks/all");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art3RestoreOrRemoveItemRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art3RestoreOrRemoveItemRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary =
                "Endpoint for permanently deleting all the temporarily removed artworks with specified type of current creator account.";
            summary.Description =
                "This endpoint is used for permanently deleting all the temporarily removed artworks with specified type of current creator account.";
            summary.ExampleRequest = new() { ArtworkType = ArtworkType.Comic, };
            summary.Response<Art3RemoveAllDeletedItemsHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { HttpCode = StatusCodes.Status200OK, }
            );
        });
    }

    public override async Task<Art3RemoveAllDeletedItemsHttpResponse> ExecuteAsync(
        Art3RestoreOrRemoveItemRequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art3RemoveAllDeletedItemsRequest
        {
            CreatorId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
        };

        var featureResponse = await FeatureExtensions.ExecuteAsync<
            Art3RemoveAllDeletedItemsRequest,
            Art3RemoveAllDeletedItemsResponse
        >(request, ct);

        var httpResponse = Art3RemoveAllDeletedItemsHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
