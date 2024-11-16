using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14;

public class G14Endpoint : Endpoint<G14RequestDto, G14HttpResponse>
{
    public override void Configure()
    {
        Get("/g14/recommended-category");
        PreProcessor<AuthPreProcessor<G14RequestDto>>();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get recommended artworks based on category for users";
            summary.Description = "This endpoint is used for get recommended artworks based on category for users";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G14HttpResponse { AppCode = G14ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G14HttpResponse> ExecuteAsync(
        G14RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();
        var g14Req = new G14Request { UserId = stateBag.AppRequest.UserId, Limit = requestDto.Limit };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G14Request, G14Response>(g14Req, ct);

        var httpResponse = G14HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g14Req, featResponse);

        if (featResponse.IsSuccess)
        {
            List<ArtworkDto> artworks = [];
            featResponse.Artworks.ForEach(artwork =>
            {
                artworks.Add(new ArtworkDto
                {
                    Id = artwork.Id,
                    Name = artwork.Title,
                    AuthorName = artwork.AuthorName,
                    CountryName = artwork.Origin.CountryName,
                    Categories = artwork.ArtworkCategories.Select(x => x.Category.Name)
                        .ToList(),
                    SeriesName = artwork.ArtworkSeries.Select(x => x.Series.Title)
                        .FirstOrDefault(),
                    HasSeries = artwork.HasSeries,
                    ArtworkStatus = artwork.ArtworkStatus.ToString(),
                    StarRates = artwork.ArtworkMetaData.AverageStarRate,
                    ViewCount = artwork.ArtworkMetaData.TotalViews,
                    FavoriteCount = artwork.ArtworkMetaData.TotalFavorites,
                    ThumbnailUrl = artwork.ThumbnailUrl,
                    Introduction = artwork.Introduction,
                    CommentCount = artwork.ArtworkMetaData.TotalComments
                });
            });
            httpResponse.Body = new G14ResponseDto
            {
                Result = artworks
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
