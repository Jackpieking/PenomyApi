using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G13;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG13.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG13.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG13;

public class G13Endpoint : Endpoint<EmptyDto, G13HttpResponse>
{
    public override void Configure()
    {
        Get("/g13/RecentlyUpdatedAnimes");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting recently updated anime.";
            summary.Description = "This endpoint is used for getting recently updated anime.";
            summary.Response<G13HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G13ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G13HttpResponse> ExecuteAsync(EmptyDto req, CancellationToken ct)
    {
        var G13Request = new G13Request();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G13Request, G13Response>(
            G13Request,
            ct
        );

        var httpResponse = G13HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(G13Request, featResponse);

        httpResponse.Body = new G13ResponseDto
        {
            ArtworkList = featResponse.ArtworkList.ConvertAll(x => new G13ResponseDtoObject()
            {
                ArtworkId = x.Id,
                Title = x.Title,
                Supplier = x.AuthorName,
                Thumbnail = x.ThumbnailUrl,
                Favorite = x.ArtworkMetaData.TotalFavorites,
                Rating = x.ArtworkMetaData.AverageStarRate,
                LastUpdateAt = x.UpdatedAt,
                FlagUrl = x.Origin.ImageUrl,
            }),
        };

        return httpResponse;
    }
}
