using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG46;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG46.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G46;

public class G46Endpoint : Endpoint<G46RequestDto, G46HttpResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public G46Endpoint(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override void Configure()
    {
        Post("/g46/favorite/add");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        DontThrowIfValidationFails();
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for add to favorite artwork list";
            summary.Description = "This endpoint is used for add to favorite artwork list";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G46HttpResponse(G46ResponseStatusCode.SUCCESS.ToString()));
        });
    }

    public override async Task<G46HttpResponse> ExecuteAsync(
        G46RequestDto requestDto,
        CancellationToken ct
    )
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
            return new G46HttpResponse
            {
                HttpCode = StatusCodes.Status401Unauthorized
            };

        var g46Req = new G46Request
        {
            ArtworkId = requestDto.ArtworkId,
            UserId = userId
        };
        var featResponse = await FeatureExtensions.ExecuteAsync<G46Request, G46Response>(g46Req, ct);

        var httpResponse = G46HttpResponseManager
            .Resolve(featResponse.AppCode)
            .Invoke(g46Req, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
