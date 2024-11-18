using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM1;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM1.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM1.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM1;

public class SM1Endpoint : Endpoint<EmptyRequest, SM1HttpResponse>
{
    public override void Configure()
    {
        Get("/SM1/user-profile/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<EmptyRequest>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get user's profile";
            summary.Description = "This endpoint is for get user's profile";
            summary.Response<SM1HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM1ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM1HttpResponse> ExecuteAsync(
        EmptyRequest requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM1Request
        {
            UserId = stateBag.AppRequest.UserId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM1Request, SM1Response>(
            featRequest,
            ct
        );

        var httpResponse = SM1ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM1ResponseDto
            {
                NickName = featResponse.Result.NickName,
                Gender = featResponse.Result.Gender,
                AvatarUrl = featResponse.Result.AvatarUrl,
                AboutMe = featResponse.Result.AboutMe,
                TotalFollowedCreators = featResponse.Result.TotalFollowedCreators,
                RegisteredAt = featResponse.Result.RegisteredAt
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
