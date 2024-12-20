using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG50;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG50.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G50.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G50;

public class G50Endpoint : Endpoint<G50RequestDto, G50HttpResponse>
{
    public override void Configure()
    {
        Post("/g50/artwork/revoke-rate");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G50AuthPreProcessor>();
        DontThrowIfValidationFails();
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for revoke artwork star";
            summary.Description = "This endpoint is used for revoke artwork star";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G50HttpResponse(G50ResponseStatusCode.SUCCESS.ToString()));
        });
    }

    public override async Task<G50HttpResponse> ExecuteAsync(
        G50RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G50StateBag>();
        var g50Req = new G50Request
        {
            ArtworkId = requestDto.ArtworkId
        };
        g50Req.SetUserId(stateBag.AppRequest.GetUserId());
        var featResponse = await FeatureExtensions.ExecuteAsync<G50Request, G50Response>(g50Req, ct);

        var httpResponse = G50HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g50Req, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
