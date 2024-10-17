using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14;

public class G14Endpoint : Endpoint<G14RequestDto, G14HttpResponse>
{
    public override void Configure()
    {
        Get("G14/AnimesByCategory/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting animes by category.";
            summary.Description = "This endpoint is used for getting animes by category.";
            summary.Response<G14HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G14ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G14HttpResponse> ExecuteAsync(G14RequestDto req, CancellationToken ct)
    {
        var G14Request = new G14Request { Category = long.Parse(req.CategoryId) };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G14Request, G14Response>(
            G14Request,
            ct
        );

        var httpResponse = G14HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G14Request, featResponse);

        httpResponse.Body = new G14ResponseDto
        {
            Category = featResponse.Result.First().Category.Name,
            ArtworkList = featResponse.Result
            .ConvertAll(x => new FeatG14ResponseDtoObject()
            {
                CategoryName = x.Category.Name,
                ArtworkId = x.Artwork.Id,
                Title = x.Artwork.Title,
                Supplier = x.Artwork.AuthorName,
                Thumbnail = x.Artwork.ThumbnailUrl,
                Favorite = x.Artwork.ArtworkMetaData.TotalFavorites,
                Rating = x.Artwork.ArtworkMetaData.AverageStarRate,
                FlagUrl = x.Artwork.Origin.ImageUrl
            }).ToList(),
        };


        return httpResponse;
    }
}
