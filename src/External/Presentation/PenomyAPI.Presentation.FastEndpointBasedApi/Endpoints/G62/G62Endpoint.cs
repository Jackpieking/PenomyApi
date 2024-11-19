using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G62;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G62;

public class G62Endpoint : Endpoint<G62RequestDto, G62HttpResponse>
{
    public override void Configure()
    {
        Post("/G62/unfollow-creator");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G62RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user unfollows a creator";
            summary.Description = "This endpoint is used for user to unfollows a creator";
            summary.Response<G62HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G62ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G62HttpResponse> ExecuteAsync(
        G62RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G62Request
        {
            UserId = stateBag.AppRequest.UserId,
            CreatorId = long.Parse(requestDto.CreatorId)
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G62Request, G62Response>(
            featRequest,
            ct
        );

        var httpResponse = G62ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G62ResponseDto { Isuccess = true };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
