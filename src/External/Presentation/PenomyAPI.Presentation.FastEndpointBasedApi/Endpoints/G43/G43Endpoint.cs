using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G43;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G43;

public class G43Endpoint : Endpoint<G43RequestDto, G43HttpResponse>
{
    private readonly ICommonCacheHandler _commonCacheHandler;

    public G43Endpoint(ICommonCacheHandler commonCacheHandler)
    {
        _commonCacheHandler = commonCacheHandler;
    }

    public override void Configure()
    {
        Post("g43/artwork/follow");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G43RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user follow artwork";
            summary.Description = "This endpoint is used for user follow artwork";
            summary.Response<G43HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G43ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G43HttpResponse> ExecuteAsync(
        G43RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G43Request
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkId = long.Parse(requestDto.ArtworkId)
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G43Request, G43Response>(
            featRequest,
            ct
        );

        var httpResponse = G43ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            // Remove cache after change detail
            await _commonCacheHandler.ClearG5MangaDetailCacheAsync(long.Parse(requestDto.ArtworkId), ct);
            httpResponse.Body = new G43ResponseDto { Isuccess = true };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
