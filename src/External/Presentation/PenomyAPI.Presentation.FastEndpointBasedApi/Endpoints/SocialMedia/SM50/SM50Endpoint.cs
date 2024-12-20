using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM50;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM50.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM50.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM50;

public class SM50Endpoint : Endpoint<SM50RequestDto, SM50HttpResponse>
{
    public override void Configure()
    {
        Post("/SM50/group/leave");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM50RequestDto>>();

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
                example: new SM50HttpResponse
                {
                    AppCode = SM50ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<SM50HttpResponse> ExecuteAsync(
        SM50RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM50Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(requestDto.GroupId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM50Request, SM50Response>(
            featRequest,
            ct
        );

        var httpResponse = SM50ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
