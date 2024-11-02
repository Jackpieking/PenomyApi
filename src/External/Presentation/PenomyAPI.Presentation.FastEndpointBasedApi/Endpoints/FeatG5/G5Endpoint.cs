using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5.Middlewares;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5;

public class G5Endpoint : Endpoint<G5Request, G5HttpResponse>
{
    public override void Configure()
    {
        Get("/g5/artwork-detail");

        PreProcessor<G5AuthPreProcessor>();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get artwork detail";
            summary.Description = "This endpoint is used for get artwork detail";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G5HttpResponse { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G5HttpResponse> ExecuteAsync(
        G5Request requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<G5StateBag>();

        var g5Req = new G5Request { Id = requestDto.Id, UserId = stateBag.AppRequest.GetUserId() };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G5Request, G5Response>(g5Req, ct);

        var httpResponse = G5HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g5Req, featResponse);

        if (featResponse.IsSuccess)
            httpResponse.Body = new G5ResponseDto
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
                IsUserFavorite = featResponse.IsArtworkFavorite
            };
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
