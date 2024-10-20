using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.APP.FeatG6;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.HttpResponse;
using ArtworkDto = PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.DTOs.ArtworkDto;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6;

public class G6Endpoint : Endpoint<G6Request, G6HttpResponse>
{
    public override void Configure()
    {
        Get("/g6/artwork-recommend");

        AllowAnonymous();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary =
                "Endpoint for get list of artworks have the same series as current artwork";
            summary.Description =
                "This endpoint is used for get list of artworks have the same series as current artwork";
            summary.Response<G6HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G6ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G6HttpResponse> ExecuteAsync(
        G6Request requestDto,
        CancellationToken ct
    )
    {
        var httpResponse = new G6HttpResponse();

        var g6Req = new G6Request { Top = requestDto.Top, };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G6Request, G6Response>(g6Req, ct);

        httpResponse = G6HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(g6Req, featResponse);

        if (featResponse.IsSuccess && featResponse.Result.Count > 0)
        {
            List<ArtworkDto> g7ResponseDtos = new();
            foreach (var response in featResponse.Result)
            {
                g7ResponseDtos.Add(
                    new ArtworkDto()
                    {
                        Id = response.Id,
                        AuthorName = response.AuthorName,
                        CategoryName = response
                            .ArtworkCategories.Select(x => x.Category.Name)
                            .FirstOrDefault(),
                        StarRates = response.ArtworkMetaData.AverageStarRate,
                        ThumbnailUrl = response.ThumbnailUrl,
                        Title = response.Title,
                    }
                );
            }
            httpResponse.Body = new G6ResponseDto { Result = g7ResponseDtos };
            return httpResponse;
        }
        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
