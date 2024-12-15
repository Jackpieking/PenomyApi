using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM49;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM49.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM49.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM49;

public class SM49Endpoint : Endpoint<SM49RequestDto, SM49HttpResponse>
{
    public override void Configure()
    {
        Post("/SM49/friends/accept");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM49RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get users friends";
            summary.Description = "This endpoint is used for user get users friends";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM49HttpResponse
                {
                    AppCode = SM49ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<SM49HttpResponse> ExecuteAsync(
        SM49RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM49Request
        {
            UserId = stateBag.AppRequest.UserId,
            FriendId = requestDto.FriendId,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM49Request, SM49Response>(
            featRequest,
            ct
        );

        var httpResponse = SM49ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
