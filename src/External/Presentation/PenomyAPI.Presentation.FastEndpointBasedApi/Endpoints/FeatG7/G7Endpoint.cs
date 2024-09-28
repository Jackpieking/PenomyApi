using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.App.FeatG7;
using PenomyAPI.App.FeatG8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG7.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG7.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG7
{
    public class G7Endpoint : Endpoint<G7Request, G7HttpResponse>
    {
        public override void Configure()
        {
            Get("/g7/artwork-recommend");

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
                summary.Response<G8HttpResponse>(
                    description: "Represent successful operation response.",
                    example: new() { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
                );
            });
        }

        public override async Task<G7HttpResponse> ExecuteAsync(
            G7Request requestDto,
            CancellationToken ct
        )
        {
            var httpResponse = new G7HttpResponse();

            try
            {
                var g7Req = new G7Request
                {
                    Id = requestDto.Id,
                    PageSize = requestDto.PageSize,
                    StartPage = requestDto.StartPage
                };

                // Get FeatureHandler response.
                var featResponse = await FeatureExtensions.ExecuteAsync<G7Request, G7Response>(
                    g7Req,
                    ct
                );

                httpResponse = G7ResponseManager
                    .Resolve(featResponse.StatusCode)
                    .Invoke(g7Req, featResponse);

                if (featResponse.IsSuccess && featResponse.Result.Count > 0)
                {
                    List<ArtworkDto> g7ResponseDtos = new();
                    foreach (var response in featResponse.Result)
                    {
                        g7ResponseDtos.Add(
                            new ArtworkDto()
                            {
                                AuthorName = response.AuthorName,
                                CategoryName = response
                                    .ArtworkCategories.Select(x => x.Category.Name)
                                    .FirstOrDefault(),
                                StarRates = (byte)(
                                    response.UserRatingArtworks.Sum(x => x.StarRates)
                                    / response.UserRatingArtworks.Count()
                                ),
                                ThumbnailUrl = response.ThumbnailUrl,
                                Title = response.Title,
                            }
                        );
                    }
                    httpResponse.Body = new G7ResponseDto { Result = g7ResponseDtos };
                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                httpResponse.Errors = new List<string> { ex.Message };
            }

            return httpResponse;
        }
    }
}
