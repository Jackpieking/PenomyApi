using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8;

public class G8Endpoint : Endpoint<G8Request, G8HttpResponse>
{
    public override void Configure()
    {
        Get("/g8/artwork-chapters");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get artwork chapters";
            summary.Description = "This endpoint is used for get artwork chapters";
            summary.Response<G8HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G8ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G8HttpResponse> ExecuteAsync(G8Request request, CancellationToken ct)
    {
        var httpResponse = new G8HttpResponse();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G8Request, G8Response>(request, ct);

        httpResponse = G8HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(request, featResponse);

        if (featResponse.IsSuccess && featResponse.Chapters.Count > 0)
        {
            List<ArtworkChapterDto> g8ResponseDtos = [];
            foreach (var chapter in featResponse.Chapters)
            {
                g8ResponseDtos.Add(
                    new ArtworkChapterDto
                    {
                        Id = chapter.Id.ToString(),
                        ChapterName = chapter.Title,
                        UploadOrder = chapter.UploadOrder,
                        CreatedTime = chapter.CreatedAt,
                        CommentCount = chapter.ChapterMetaData.TotalComments,
                        FavoriteCount = chapter.ChapterMetaData.TotalFavorites,
                        ViewCount = chapter.ChapterMetaData.TotalViews,
                        ThumbnailUrl = chapter.ThumbnailUrl,
                        AllowComment = chapter.AllowComment,
                    }
                );
            }

            httpResponse.Body = new G8ResponseDto
            {
                Result = g8ResponseDtos,
                ChapterCount = featResponse.ChapterCount
            };

            return httpResponse;
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
