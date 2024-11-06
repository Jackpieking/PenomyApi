using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG47;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G47.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47;

public class G47Endpoint : Endpoint<G47RequestDto, G47HttpResponse>
{
    public override void Configure()
    {
        Post("/g47/favorite/remove");
        PreProcessor<G47AuthPreProcessor>();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for remove favorite artwork list";
            summary.Description = "This endpoint is used for remove from favorite artwork list";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G47HttpResponse(G47ResponseStatusCode.SUCCESS.ToString())
            );
        });
    }

    public override async Task<G47HttpResponse> ExecuteAsync(
        G47RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G47StateBag>();
        var g47Req = new G47Request { ArtworkId = requestDto.ArtworkId, UserId = stateBag.AppRequest.GetUserId() };
        var featResponse = await FeatureExtensions.ExecuteAsync<G47Request, G47Response>(
            g47Req,
            ct
        );

        var httpResponse = G47HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g47Req, featResponse);
        if (featResponse.AppCode is G47ResponseStatusCode.SUCCESS)
            httpResponse.FavoriteCount = featResponse.FavoriteCount;
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
