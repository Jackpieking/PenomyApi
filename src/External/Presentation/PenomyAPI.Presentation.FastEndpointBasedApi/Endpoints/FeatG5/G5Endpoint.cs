using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5;

public class G5Endpoint : Endpoint<G5Request, G5HttpResponse>
{
    public override void Configure()
    {
        Get("/g5/artwork-detail");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get artwork detail";
            summary.Description = "This endpoint is used for get artwork detail";
            summary.Response<G5HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G5HttpResponse> ExecuteAsync(
        G5Request requestDto,
        CancellationToken ct
    )
    {
        var httpResponse = new G5HttpResponse();

        var g5req = new G5Request { Id = requestDto.Id };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G5Request, G5Response>(g5req, ct);

        httpResponse = G5ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g5req, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new DTOs.G5ResponseDto
            {
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
                ThumbnailUrl = featResponse.Result.ThumbnailUrl
            };
        }

        return httpResponse;
    }
}
