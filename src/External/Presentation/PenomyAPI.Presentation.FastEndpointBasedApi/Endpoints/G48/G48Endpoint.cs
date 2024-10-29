using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G48;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48;

public class G48Endpoint : Endpoint<G48Request, G48HttpResponse>
{
    public override void Configure()
    {
        Get("/G48/favorite-artworks");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get favorite artworks";
            summary.Description = "This endpoint is used for user get favorite artworks";
            summary.Response<G48HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G48ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G48HttpResponse> ExecuteAsync(
        G48Request requestDto,
        CancellationToken ct
    )
    {
        var featRequest = new G48Request
        {
            UserId = requestDto.UserId,
            ArtworkType = requestDto.ArtworkType,
            ArtNum = requestDto.ArtNum,
            PageNum = requestDto.PageNum
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G48Request, G48Response>(
            featRequest,
            ct
        );

        var httpResponse = G48ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new ArtworkCardDto
            {
                Artworks = featResponse.Result.Select(o => new ArtworkDto
                {
                    Id = o.Id.ToString(),
                    Title = o.Title,
                    CreatedBy = o.CreatedBy.ToString(),
                    AuthorName = o.AuthorName,
                    ThumbnailUrl = o.ThumbnailUrl,
                    TotalFavorites = o.ArtworkMetaData.TotalFavorites,
                    AverageStarRate = o.ArtworkMetaData.AverageStarRate,
                    OriginUrl = o.Origin.ImageUrl,
                    Chapters = o.Chapters
                        .Select(c => new ChapterDto
                        {
                            Id = c.Id.ToString(),
                            Title = c.Title,
                            UploadOrder = c.UploadOrder,
                            Time = c.UpdatedAt
                        }).ToList()
                })
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
