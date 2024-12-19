using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Cache;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5;

public class G5Endpoint : Endpoint<G5RequestDto, G5HttpResponse>
{
    private readonly ICommonCacheHandler _cacheHandler;

    public G5Endpoint(ICommonCacheHandler cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override void Configure()
    {
        Get("/g5/artwork-detail");

        AllowAnonymous();
        PreProcessor<G5AuthPreProcessor>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get comic detail";
            summary.Description = "This endpoint is used for get comic detail";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G5HttpResponse { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G5HttpResponse> ExecuteAsync(
        G5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G5StateBag>();

        var httpResponse = await _cacheHandler.GetOrSetG5MangaDetailCacheAsync(
            stateBag,
            requestDto,
            ct
        );

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
