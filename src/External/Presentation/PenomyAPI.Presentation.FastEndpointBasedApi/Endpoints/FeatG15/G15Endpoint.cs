using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG15;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG15.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG15.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG115;

public class G15Endpoint : Endpoint<G15Request, G15HttpResponse>
{
    public override void Configure()
    {
        Get("/g15/anime-detail");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get anime detail";
            summary.Description = "This endpoint is used for get anime detail";
            summary.Response<G15HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G15ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G15HttpResponse> ExecuteAsync(
        G15Request requestDto,
        CancellationToken ct
    )
    {
        var httpResponse = new G15HttpResponse();

        var g15req = new G15Request { Id = requestDto.Id };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G15Request, G15Response>(g15req, ct);

        httpResponse = G15HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g15req, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new G15ResponseDto
            {
                Id = featResponse.Result.Id,
                Name = featResponse.Result.Title,
                AuthorName = featResponse.Result.AuthorName,
                CountryName = featResponse.Result.Origin.CountryName,
                Categories = featResponse
                    .Result.ArtworkCategories.Select(x => x.Category.Name)
                    .ToList(),
                SeriesName = featResponse
                    .Result.ArtworkSeries.Select(x => x.Series.Title)
                    .FirstOrDefault(),
                HasSeries = featResponse.Result.HasSeries,
                ArtworkStatus = featResponse.Result.ArtworkStatus.ToString(),
                StarRates = featResponse.Result.ArtworkMetaData.AverageStarRate,
                ViewCount = featResponse.Result.ArtworkMetaData.TotalViews,
                FavoriteCount = featResponse.Result.ArtworkMetaData.TotalFavorites,
                ThumbnailUrl = featResponse.Result.ThumbnailUrl,
                Introduction = featResponse.Result.Introduction,
                CommentCount = featResponse.Result.ArtworkMetaData.TotalComments,
            };
        }
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
