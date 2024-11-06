using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG28;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28;

public class G28AuthEndpoint : Endpoint<G28Request, G28HttpResponse>
{
    public override void Configure()
    {
        Get("/g28/user-profile/created-artworks/");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G28PreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting createdartworks for authenticated user.";
            summary.Description =
                "This endpoint is used for getting created artworks for authenticated user.";
            summary.Response<G28HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G28ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G28HttpResponse> ExecuteAsync(G28Request req, CancellationToken ct)
    {
        var stateBag = ProcessorState<G28StateBag>();

        req.SetUserId(stateBag.AppRequest.GetUserId());

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G28Request, G28Response>(req, ct);

        var httpResponse = G28HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(req, featResponse);

        httpResponse.Body = new G28ResponseDto
        {
            ArtworkList = featResponse.result.ConvertAll(x => new G28ResponseDtoObject()
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
