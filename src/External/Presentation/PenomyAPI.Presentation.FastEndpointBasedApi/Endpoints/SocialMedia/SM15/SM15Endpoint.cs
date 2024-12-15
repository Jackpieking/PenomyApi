using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM15;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM15.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15;

public class Sm15Endpoint : Endpoint<EmptyRequest, Sm15HttpResponse>
{
    public override void Configure()
    {
        Get("/SM15/posts/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<EmptyRequest>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get created post";
            summary.Description = "This endpoint is used for user get created post";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Sm15HttpResponse
                {
                    AppCode = SM15ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<Sm15HttpResponse> ExecuteAsync(
        EmptyRequest requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM15Request { UserId = stateBag.AppRequest.UserId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM15Request, SM15Response>(
            featRequest,
            ct
        );

        var httpResponse = SM15ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        httpResponse.Body = new SM15ResponseDto();
        httpResponse.Body.UserPosts = new List<UserPostDto>();
        if (featResponse.StatusCode == SM15ResponseStatusCode.SUCCESS)
            foreach (var p in featResponse.UserPosts)
            {
                var userPostDto = new UserPostDto
                {
                    Id = p.Id.ToString(),
                    Content = p.Content,
                    CreatedBy = p.Creator.NickName,
                    CreatedAt = p.CreatedAt,
                    AllowComment = p.AllowComment,
                    PublicLevel = p.PublicLevel,
                    TotalLikes = p.TotalLikes,
                    UserAvatar = p.Creator.AvatarUrl,
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
                httpResponse.Body.UserPosts.Add(userPostDto);
            }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
