using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM32;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32.HttpResponsse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32;

public class SM32Endpoint : Endpoint<EmptyRequest, SM32HttpResponse>
{
    public override void Configure()
    {
        Get("/SM32/friends/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<EmptyRequest>>();

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
                example: new SM32HttpResponse
                {
                    AppCode = SM32ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<SM32HttpResponse> ExecuteAsync(
        EmptyRequest empty,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM32Request { UserId = stateBag.AppRequest.UserId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM32Request, SM32Response>(
            featRequest,
            ct
        );

        var httpResponse = SM32ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.StatusCode == SM32ResponseStatusCode.SUCCESS)
            httpResponse.Body = new SM32ResponseDto
            {
                Users = featResponse.UserProfiles.Select(x => new UserResponseDto
                {
                    UserId = x.UserId,
                    NickName = x.NickName,
                    AvatarUrl = x.AvatarUrl,
                    Gender = x.Gender,
                    AboutMe = x.AboutMe,
                }),
            };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
