using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.HttpResponseManagers;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1;

public sealed class Art1Endpoint : Endpoint<Art1RequestDto, Art1HttpResponse>
{
    public override void Configure()
    {
        Get("art1/artworks");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art1RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art1RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status400BadRequest);
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for creator to get the created artworks.";
            summary.Description = "This endpoint is used by creator to get the created artworks.";
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

    public override async Task<Art1HttpResponse> ExecuteAsync(
        Art1RequestDto requestDto,
        CancellationToken ct
    )
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var featRequest = requestDto.MapToRequest(
            creatorId: creatorId,
            pageSize: Art1PaginationOptions.DEFAULT_PAGE_SIZE
        );

        var featResponse = await FeatureExtensions.ExecuteAsync<Art1Request, Art1Response>(
            featRequest,
            ct
        );

        var httpResponse = Art1HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
