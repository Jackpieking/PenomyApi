using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG59;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG59.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G59.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G59.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG59;

public class G59AnonymousEndpoint : Endpoint<G59Request, G59HttpResponse>
{
    public override void Configure()
    {
        Get("/g59/replycomment/get/anonymous");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting artwork reply comment for anonymous user.";
            summary.Description =
                "This endpoint is used for getting artwork reply comment for anonymous user.";
            summary.Response<G59HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G59ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G59HttpResponse> ExecuteAsync(G59Request req, CancellationToken ct)
    {
        // Set fake user id.
        var stateBag = ProcessorState<G59StateBag>();
        req.SetUserId("1");

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G59Request, G59Response>(req, ct);

        var httpResponse = G59HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        httpResponse.Body = new G59ResponseDto
        {
            CommentList = featResponse.Result.ConvertAll(x => new G59ResponseDtoObject
            {
                Id = x.Id.ToString(),
                Content = x.Content,
                PostDate = x.CreatedAt.ToString("MMMM dd, yyyy"),
                Username = x.Creator.NickName,
                Avatar = x.Creator.AvatarUrl,
                TotalReplies = x.TotalChildComments,
                LikeCount = x.TotalLikes,
                IsArtworkAuthor = false,
                IsCommentAuthor = false,
                CreatedBy = x.Creator?.UserId.ToString(),
                IsLiked = x.UserLikeArtworkComment.Count() > 0,
            }),
        };

        return httpResponse;
    }
}
