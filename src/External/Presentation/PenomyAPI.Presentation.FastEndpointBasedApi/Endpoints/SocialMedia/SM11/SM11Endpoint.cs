using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM11;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM11.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM11.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM11.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM11;

public class Sm11Endpoint : Endpoint<SM11RequestDto, Sm11HttpResponse>
{
    public override void Configure()
    {
        Get("/SM11/group-posts/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<SM11RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get created group post";
            summary.Description = "This endpoint is used for user get created group post";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Sm11HttpResponse
                {
                    AppCode = SM11ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<Sm11HttpResponse> ExecuteAsync(
        SM11RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM11Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(requestDto.GroupId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM11Request, SM11Response>(
            featRequest,
            ct
        );

        var httpResponse = SM11ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        httpResponse.Body = new SM11ResponseDto();
        httpResponse.Body.GroupPosts = new List<GroupPostDto>();
        if (featResponse.StatusCode == SM11ResponseStatusCode.SUCCESS)
            foreach (var p in featResponse.GroupPosts)
            {
                var groupPostDto = new GroupPostDto
                {
                    Id = p.Id.ToString(),
                    UserNickName = p.Creator.NickName,
                    UserAvatar = p.Creator.AvatarUrl,
                    GroupId = p.Group.Id.ToString(),
                    GroupName = p.Group.Name,
                    GroupAvatar = p.Group.CoverPhotoUrl,
                    Content = p.Content,
                    CreatedBy = p.Creator.UserId.ToString(),
                    CreatedAt = p.CreatedAt.ToString("dd/MM/yyyy"),
                    AllowComment = p.AllowComment,
                    TotalLikes = p.TotalLikes,
                    HasLikedPost = p.UserLikes.ToList().Count > 0,
                    AttachedMedias = p
                        .AttachedMedias.Select(m => new AttachMediaDto
                        {
                            FileName = m.FileName,
                            MediaType = m.MediaType,
                            StorageUrl = m.StorageUrl,
                            UploadOrder = m.UploadOrder,
                        })
                        .ToList(),
                };
                httpResponse.Body.GroupPosts.Add(groupPostDto);
            }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
