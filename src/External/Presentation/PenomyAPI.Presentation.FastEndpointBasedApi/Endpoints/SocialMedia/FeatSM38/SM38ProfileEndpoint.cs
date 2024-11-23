using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM38;
using PenomyAPI.App.SM38.GroupProfile;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Profile.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Profile.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Profile;

public class SM38ProfileEndpoint : Endpoint<SM38ProfileRequestDto, SM38ProfileHttpResponse>
{
    public override void Configure()
    {
        Post("sm38/group-profile/update");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM38ProfileRequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for updating group profile.";
            summary.Description = "This endpoint is used for updating group profile.";
            summary.Response<SM38ProfileHttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM38ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM38ProfileHttpResponse> ExecuteAsync(
        SM38ProfileRequestDto req,
        CancellationToken ct
    )
    {
        SM38ProfileHttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        var SM38ProfileRequest = new SM38ProfileRequest
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(req.GroupId),
            Name = req.Name,
            Description = req.Description,
            RequireApprovedWhenPost = req.RequireApprovedWhenPost,
            SocialGroupStatus = req.SocialGroupStatus,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<
            SM38ProfileRequest,
            SM38ProfileResponse
        >(SM38ProfileRequest, ct);

        httpResponse = SM38ProfileHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM38ProfileResponseDto { IsSuccess = featResponse.Result };

            return httpResponse;
        }

        return httpResponse;
    }
}
