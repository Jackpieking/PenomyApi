using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG20;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G20.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G20.DTOs;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G20Endpoint : Endpoint<G20Request, G20HttpResponse>
{
    public override void Configure()
    {
        Get("/g20/ArtworkComment/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting artwork comment.";
            summary.Description = "This endpoint is used for getting artwork comment.";
            summary.Response<G20HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G20ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G20HttpResponse> ExecuteAsync(G20Request req, CancellationToken ct)
    {
        var featG20Request = new G20Request { ArtworkId = req.ArtworkId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G20Request, G20Response>(req, ct);

        var httpResponse = G20HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG20Request, featResponse);

        httpResponse.Body = new G20ResponseDto
        {
            CommentList = featResponse.Result.ConvertAll(x => new G20ResponseDtoObject
            {
                Id = x.Id,
                Content = x.Content,
                PostDate = x.CreatedAt.ToString("MMMM dd, yyyy"),
                Username = x.Creator.NickName,
                Avatar = x.Creator.AvatarUrl,
                TotalReplies = x.TotalChildComments,
                LikeCount = x.TotalLikes,
                IsAuthor = true
            })
        };


        return httpResponse;
    }
}
