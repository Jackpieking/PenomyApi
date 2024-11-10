using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG49;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG49.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G49;

public class G49Endpoint : Endpoint<G49RequestDto, G49HttpResponse>
{
    public override void Configure()
    {
        Post("/g49/artwork/rate");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G49AuthPreProcessor>();
        DontThrowIfValidationFails();
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for add to rate artwork star";
            summary.Description = "This endpoint is used for rating artwork star";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G49HttpResponse(G49ResponseStatusCode.SUCCESS.ToString()));
        });
    }

    public override async Task<G49HttpResponse> ExecuteAsync(
        G49RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G49StateBag>();
        var g49Req = new G49Request
        {
            ArtworkId = requestDto.ArtworkId,
            UserId = stateBag.AppRequest.GetUserId(),
            StarRate = requestDto.StarRate
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<G49Request, G49Response>(g49Req, ct);

        var httpResponse = G49HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g49Req, featResponse);
        if (featResponse.AppCode is G49ResponseStatusCode.SUCCESS)
            httpResponse.Body = new G49ResponseDto
            {
                StarRate = Math.Round(featResponse.StarRate, 2),
                CurrentUserRate = featResponse.CurrentUserRate,
                TotalUsersRate = featResponse.TotalUsersRate,
                TotalStarRate = featResponse.TotalStarRate
            };
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
