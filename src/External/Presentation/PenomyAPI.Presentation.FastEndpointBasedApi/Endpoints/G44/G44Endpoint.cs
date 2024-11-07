using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G44;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G44;

public class G44Endpoint : Endpoint<G44RequestDto, G44HttpResponse>
{
    public override void Configure()
    {
        Post("/profile/user/unfollowed-artworks");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G44RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user unfollow artwork";
            summary.Description = "This endpoint is used for user unfollow artwork";
            summary.Response<G44HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G44ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G44HttpResponse> ExecuteAsync(
        G44RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G44Request
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkId = requestDto.ArtworkId,
            ArtworkType = requestDto.ArtworkType,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G44Request, G44Response>(
            featRequest,
            ct
        );

        var httpResponse = G44ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G44ResponseDto { Isuccess = true };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
