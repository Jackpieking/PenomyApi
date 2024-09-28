using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG5;
using PenomyAPI.App.FeatG8;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8
{
    public class G8Endpoint : Endpoint<G8Request, G8HttpResponse>
    {
        public override void Configure()
        {
            Get("/g5/artwork-chapters");

            AllowAnonymous();

            Description(builder: builder =>
            {
                builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
            });

            Summary(endpointSummary: summary =>
            {
                summary.Summary = "Endpoint for get artwork chapters";
                summary.Description = "This endpoint is used for get artwork chapters";
                summary.Response<G8HttpResponse>(
                    description: "Represent successful operation response.",
                    example: new() { AppCode = G5ResponseStatusCode.SUCCESS.ToString() }
                );
            });
        }

        public override async Task<G8HttpResponse> ExecuteAsync(
            G8Request requestDto,
            CancellationToken ct
        )
        {
            var httpResponse = new G8HttpResponse();

            try
            {
                var g8Req = new G8Request
                {
                    Id = requestDto.Id,
                    PageSize = requestDto.PageSize,
                    StartPage = requestDto.StartPage
                };

                // Get FeatureHandler response.
                var featResponse = await FeatureExtensions.ExecuteAsync<G8Request, G8Response>(
                    g8Req,
                    ct
                );

                httpResponse = G8ResponseManager
                    .Resolve(featResponse.StatusCode)
                    .Invoke(g8Req, featResponse);

                if (featResponse.IsSuccess && featResponse.Result.Count > 0)
                {
                    List<ArtworkChapterDto> g8ResponseDtos = [];
                    foreach (var chapter in featResponse.Result)
                    {
                        g8ResponseDtos.Add(
                            new ArtworkChapterDto
                            {
                                ChapterName = chapter.Title,
                                UploadOrder = chapter.UploadOrder,
                                CreatedTime = chapter.CreatedAt,
                                CommentCount = chapter.TotalComments,
                                FavoriteCount = chapter.TotalFavorites,
                                ViewCount = chapter.TotalViews
                            }
                        );
                    }
                    httpResponse.Body = new G8ResponseDto { Result = g8ResponseDtos };
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
