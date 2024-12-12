using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.App.SM6;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM6.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM6.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM6.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM6;

public class SM6Endpoint : Endpoint<SM6RequestDto, SM6HttpResponse>
{
    private readonly INotificationHub _notificationHub;

    public SM6Endpoint(INotificationHub notificationHub)
    {
        _notificationHub = notificationHub;
    }

    public override void Configure()
    {
        Get("/SM6/group/request");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM6RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user send join group request";
            summary.Description = "This endpoint is for user send join group request";
            summary.Response<SM6HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM6ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM6HttpResponse> ExecuteAsync(
        SM6RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM6Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(requestDto.GroupId)
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM6Request, SM6Response>(
            featRequest,
            ct
        );

        var httpResponse = SM6ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM6ResponseDto
            {
                IsSuccess = featResponse.IsSuccess,
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
