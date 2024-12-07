using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3.OtherHandlers.RestoreAllDeletedItems;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3RestoreAllDeletedItemsEndpoint
    : Endpoint<Art3RestoreOrRemoveItemRequestDto, Art3RestoreAllDeletedItemsHttpResponse>
{
    public override void Configure()
    {
        Post("art3/restore/artworks/all");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art3RestoreOrRemoveItemRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art3RestoreOrRemoveItemRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for restoring all the temporarily removed artworks with specified type of current creator account.";
            summary.Description = "This endpoint is used for restoring all the temporarily removed artworks with specified type of current creator account.";
            summary.ExampleRequest = new() { ArtworkType = ArtworkType.Comic, };
            summary.Response<Art3RemoveAllDeletedItemsHttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<Art3RestoreAllDeletedItemsHttpResponse> ExecuteAsync(
        Art3RestoreOrRemoveItemRequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art3RestoreAllDeletedItemsRequest
        {
            CreatorId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art3RestoreAllDeletedItemsRequest, Art3RestoreAllDeletedItemsResponse>(request, ct);

        var httpResponse = Art3RestoreAllDeletedItemsHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
