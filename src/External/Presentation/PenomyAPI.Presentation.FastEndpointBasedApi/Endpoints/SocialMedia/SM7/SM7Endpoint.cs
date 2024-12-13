using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM7;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM7.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM7;

public class SM7Endpoint : Endpoint<SM7RequestDto, SM7HttpResponse>
{
    public override void Configure()
    {
        Get("/SM7/groups/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM7RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get joined groups";
            summary.Description = "This endpoint is used for user get joined groups";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM7HttpResponse { AppCode = SM7ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM7HttpResponse> ExecuteAsync(
        SM7RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM7Request
        {
            UserId = stateBag.AppRequest.UserId,
            PageNum = requestDto.PageNum,
            GroupNum = requestDto.GroupNum,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM7Request, SM7Response>(
            featRequest,
            ct
        );

        var httpResponse = SM7ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
            httpResponse.Body = new SM7ResponseDto
            {
                Groups = featResponse.Result.Select(o => new GroupDto
                {
                    Id = o.Id.ToString(),
                    Name = o.Name,
                    IsPublic = o.IsPublic,
                    Description = o.Description,
                    CoverPhotoUrl = o.CoverPhotoUrl,
                    TotalMembers = o.TotalMembers,
                    RequireApprovedWhenPost = o.RequireApprovedWhenPost,
                    GroupStatus = o.GroupStatus,
                    CreatedBy = o.CreatedBy.ToString(),
                    CreatedAt = o.CreatedAt,
                    ActivityTime = o.Creator.UpdatedAt,
                }),
            };

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
