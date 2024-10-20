using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G45;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45;

public class G45Endpoint : Endpoint<G45Request, G45HttpResponse>
{
    public override void Configure()
    {
        Get("/g45/followed-artworks");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get followed artworks";
            summary.Description = "This endpoint is used for user get followed artworks";
            summary.Response<G45HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G45ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G45HttpResponse> ExecuteAsync(
        G45Request requestDto,
        CancellationToken ct
    )
    {
        var featRequest = new G45Request
        {
            UserId = requestDto.UserId,
            ArtworkType = requestDto.ArtworkType,
            ArtNum = requestDto.ArtNum,
            PageNum = requestDto.PageNum
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G45Request, G45Response>(
            featRequest,
            ct
        );

        var httpResponse = G45ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G45ResponseDto
            {
                Artworks = featResponse.Result.Select(o => new ArtworkDto
                {
                    Title = o.Title,
                    CreatedBy = o.CreatedBy.ToString(),
                    AuthorName = o.AuthorName,
                    ThumbnailUrl = o.ThumbnailUrl,
                    TotalFavorites = o.ArtworkMetaData.TotalFavorites,
                    TotalStarRates = o.ArtworkMetaData.TotalStarRates,
                    OriginUrl = o.Origin.ImageUrl,
                    Chapters = o.Chapters
                        .Select(c => new ChapterDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            UploadOrder = c.UploadOrder,
                            UpdatedAt = c.UpdatedAt
                        }).ToList()
                })
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
