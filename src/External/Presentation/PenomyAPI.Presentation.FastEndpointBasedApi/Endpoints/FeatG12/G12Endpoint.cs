using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG12;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG12.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG12.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG12;

public class G12Endpoint : Endpoint<G12RequestDto, G12HttpResponse>
{
    public override void Configure()
    {
        Get("g12/AnimesByCategory/get");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting comics by category.";
            summary.Description = "This endpoint is used for getting comics by category.";
            summary.Response<G12HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G12ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G12HttpResponse> ExecuteAsync(
        G12RequestDto req,
        CancellationToken ct
    )
    {
        var G12Request = new G12Request {};

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G12Request, G12Response>(
            G12Request,
            ct
        );

        var httpResponse = G12HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G12Request, featResponse);

        httpResponse.Body = new G12ResponseDto
        {
            Category = featResponse.Result.First().Category.Name,
            ArtworkList = featResponse.Result.ConvertAll(x => new FeatG12ResponseDtoObject()
            {
                CategoryName = x.Category.Name,
                ArtworkId = x.Artwork.Id,
                Title = x.Artwork.Title,
                Supplier = x.Artwork.AuthorName,
                Thumbnail = x.Artwork.ThumbnailUrl,
                Favorite = x.Artwork.ArtworkMetaData.TotalFavorites,
                Rating = x.Artwork.ArtworkMetaData.AverageStarRate,
                FlagUrl = x.Artwork.Origin.ImageUrl,
            }),
        };

        return httpResponse;
    }
}
