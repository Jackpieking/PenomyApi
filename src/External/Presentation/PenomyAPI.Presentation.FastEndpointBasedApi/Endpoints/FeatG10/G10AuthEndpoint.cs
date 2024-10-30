using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG10;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10;

public class G10AuthEndpoint : Endpoint<G10Request, G10HttpResponse>
{
    public override void Configure()
    {
        Get("/g10/ArtworkComment/get/");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G10AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting artwork comment for authenticated user.";
            summary.Description = "This endpoint is used for getting artwork comment for authenticated user.";
            summary.Response<G10HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G10ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G10HttpResponse> ExecuteAsync(G10Request req, CancellationToken ct)
    {
        var stateBag = ProcessorState<G10StateBag>();

        req.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G10Request, G10Response>(req, ct);

        var httpResponse = G10HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        httpResponse.Body = new G10ResponseDto
        {
            CommentList = featResponse.Result.ConvertAll(x => new G10ResponseDtoObject
            {
                Id = x.Id.ToString(),
                Content = x.Content,
                PostDate = x.CreatedAt.ToString("MMMM dd, yyyy"),
                Username = x.Creator.NickName,
                Avatar = x.Creator.AvatarUrl,
                TotalReplies = x.TotalChildComments,
                LikeCount = x.TotalLikes,
                IsArtworkAuthor = true,
                IsCommentAuthor = long.Parse(stateBag.AppRequest.GetUserId()) == x.Creator?.UserId,
                CreatedBy = x.Creator?.UserId.ToString(),
                IsLiked = x.UserLikeArtworkComment.Count() > 0,
            }),
        };

        return httpResponse;
    }
}
