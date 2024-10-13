using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG3;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse;
using System.Threading;
using System.Threading.Tasks;
namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3;

public class G3Endpoint : Endpoint<EmptyDto, FeatG3HttpResponse>
{
    public override void Configure()
    {
        Get("/g3/RecentlyUpdatedComics");
        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting recently updated comic.";
            summary.Description = "This endpoint is used for getting recently updated comic.";
            summary.Response<FeatG3HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = FeatG3ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<FeatG3HttpResponse> ExecuteAsync(EmptyDto req, CancellationToken ct)
    {
        var featG3Request = new FeatG3Request();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<FeatG3Request, FeatG3Response>(featG3Request, ct);

        var httpResponse = G3HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featG3Request, featResponse);


        httpResponse.Body = new G3ResponseDto
        {
            ArtworkList = featResponse.ArtworkList
            .ConvertAll(x => new FeatG3ResponseDtoObject()
            {
                ArtworkId = x.Id,
                Title = x.Title,
                Supplier = x.AuthorName,
                Thumbnail = x.ThumbnailUrl,
                Favorite = x.ArtworkMetaData.TotalFavorites,
                Rating = x.ArtworkMetaData.AverageStarRate,
                LastUpdateAt = x.UpdatedAt,
                FlagUrl = x.Origin.ImageUrl
            }),
        };

        return httpResponse;

    }
}
