using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG21;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G21.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G21.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G21Endpoint : Endpoint<G21RequestDto, G21HttpResponse>
{
    public override void Configure()
    {
        Get("/g21/anime-chapter-comment/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting artwork comment.";
            summary.Description = "This endpoint is used for getting artwork comment.";
            summary.Response<G21HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G21ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G21HttpResponse> ExecuteAsync(
        G21RequestDto req,
        CancellationToken ct
    )
    {
        var featG21Request = new G21Request { ChapterId = long.Parse(req.ChapterId) };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G21Request, G21Response>(
            featG21Request,
            ct
        );

        var httpResponse = G21HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG21Request, featResponse);

        httpResponse.Body = new G21ResponseDto
        {
            CommentList = featResponse.Result.ConvertAll(x => new G21ResponseDtoObject
            {
                Id = x.Id,
                Content = x.Content,
                PostDate = x.CreatedAt.ToString("MMMM dd, yyyy"),
                Username = x.Creator.NickName,
                Avatar = x.Creator.AvatarUrl,
                TotalReplies = x.TotalChildComments,
                LikeCount = x.TotalLikes,
                IsAuthor = true,
            }),
        };

        return httpResponse;
    }
}
