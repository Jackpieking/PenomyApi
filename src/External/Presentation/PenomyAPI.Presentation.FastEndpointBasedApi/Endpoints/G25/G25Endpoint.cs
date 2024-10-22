using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25Endpoint : Endpoint<G25Request, G25HttpResponse>
{
    public override void Configure()
    {
        Get("/profile/user/history");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get artworks viewed history";
            summary.Description = "This endpoint is used for get artworks viewed history.";
            summary.Response<G25HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G25HttpResponse> ExecuteAsync(
        G25Request request,
        CancellationToken ct
    )
    {
        var featRequest = new G25Request
        {
            UserId = request.UserId,
            ArtworkType = request.ArtworkType,
            PageNum = request.PageNum,
            ArtNum = request.ArtNum
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<G25Request, G25Response>(
            featRequest,
            ct
        );

        G25HttpResponse httpResponse = G25ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new ArtworkCardDto
            {
                Artworks = featResponse.Result.Select(grp => new ArtworkDto
                {
                    Id = grp.First().ArtworkId.ToString(),
                    Title = grp.First().Artwork.Title,
                    CreatedBy = grp.First().Artwork.CreatedBy.ToString(),
                    AuthorName = grp.First().Artwork.AuthorName,
                    ThumbnailUrl = grp.First().Artwork.ThumbnailUrl,
                    TotalFavorites = grp.First().Artwork.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = grp.First().Artwork.ArtworkMetaData.GetAverageStarRate(),
                    OriginUrl = grp.First().Artwork.Origin.ImageUrl,
                    Chapters = grp.Select(o => new ChapterDto
                    {
                        Id = o.Chapter.Id.ToString(),
                        Title = o.Chapter.Title,
                        UploadOrder = o.Chapter.UploadOrder,
                        Time = o.ViewedAt
                    })
                }),
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
