using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG47;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG47;

public class G47Endpoint : Endpoint<G47RequestDto, G47HttpResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public G47Endpoint(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override void Configure()
    {
        Post("/g47/favorite/remove");

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
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
            return new G47HttpResponse { HttpCode = StatusCodes.Status401Unauthorized };

        var g47Req = new G47Request { ArtworkId = requestDto.ArtworkId, UserId = 2 };
        var featResponse = await FeatureExtensions.ExecuteAsync<G47Request, G47Response>(
            g47Req,
            ct
        );

        var httpResponse = G47HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g47Req, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
