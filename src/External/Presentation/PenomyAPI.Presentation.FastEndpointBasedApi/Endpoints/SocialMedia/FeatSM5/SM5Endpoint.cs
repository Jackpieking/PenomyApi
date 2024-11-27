using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5;

public class SM5Endpoint : Endpoint<SM5RequestDto, SM5HttpResponse>
{
    public override void Configure()
    {
        Get("/sm5/group-description/get");
        AllowAnonymous();
        PreProcessor<SM5AuthPreProcessor>();
        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get group description";
            summary.Description = "This endpoint is used for get group description";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM5HttpResponse { AppCode = SM5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM5HttpResponse> ExecuteAsync(
        SM5RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<SM5StateBag>();

        var SM5Req = new SM5Request
        {
            UserId = stateBag.UserId,
            GroupId = long.Parse(requestDto.GroupId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM5Request, SM5Response>(
            SM5Req,
            ct
        );

        var httpResponse = SM5HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM5ResponseDto
            {
                Name = featResponse.Group.Name,
                IsPublic = featResponse.Group.IsPublic,
                Description = featResponse.Group.Description,
                CoverPhotoUrl = featResponse.Group.CoverPhotoUrl,
                TotalMembers = featResponse.Group.TotalMembers,
                CreatedAt = featResponse.Group.CreatedAt.ToString("dd/MM/yyyy"),
                RequireApprovedWhenPost = featResponse.Group.RequireApprovedWhenPost,
                HasJoin = featResponse.Group.GroupMembers.Count() > 0,
                IsManager = stateBag.UserId == featResponse.Group.Creator.UserId,
                ManagerName = featResponse.Group.Creator.NickName,
                HasRequestJoin =
                    featResponse.Group.SocialGroupJoinRequests != null
                    && featResponse.Group.SocialGroupJoinRequests.Any(r =>
                        r.RequestStatus == RequestStatus.Pending
                    ),
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
