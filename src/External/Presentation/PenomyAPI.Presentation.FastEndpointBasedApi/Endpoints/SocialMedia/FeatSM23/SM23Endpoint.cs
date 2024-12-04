using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM23;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.Middlewares.Authorization;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23;

public class SM23Endpoint : Endpoint<SM23RequestDto, SM23HttpResponse>
{
    public override void Configure()
    {
        Get("sm23/post-comments/get");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM23AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting user post comments.";
            summary.Description = "This endpoint is used for getting user post comments.";
            summary.Response<SM23HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM23ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM23HttpResponse> ExecuteAsync(
        SM23RequestDto req,
        CancellationToken ct
    )
    {
        SM23HttpResponse httpResponse;

        var stateBag = ProcessorState<SM23StateBag>();

        var featRequest = new SM23Request
        {
            UserId = long.Parse(stateBag.AppRequest.GetUserId()),
            PostId = long.Parse(req.PostId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM23Request, SM23Response>(
            featRequest,
            ct
        );

        httpResponse = SM23HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        if (featResponse.StatusCode == SM23ResponseStatusCode.SUCCESS)
            httpResponse.Body = new SM23ResponseDto
            {
                Comments = featResponse.Comments.ConvertAll(x => new SM23ResponseObjectDto
                {
                    Id = x.Id.ToString(),
                    Content = x.Content,
                    LikeCount = x.TotalLikes,
                    IsCommentAuthor = x.CreatedBy == long.Parse(stateBag.AppRequest.GetUserId()),
                    Avatar = x.Creator.AvatarUrl,
                    Username = x.Creator.NickName,
                    PostDate = x.CreatedAt.ToString("dd/MM/yyyy"),
                    TotalReplies = x.TotalChildComments,
                    IsLiked = x.UserLikes.ToList().Count > 0,
                }),
            };
        return httpResponse;
    }
}
