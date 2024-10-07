using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25
{
    public class G25Endpoint : Endpoint<G25Request, G25HttpResponse>
    {
        public override void Configure()
        {
            Get("/profile/user/history");

            AllowAnonymous();

            Description(builder: builder =>
            {
                builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
            });

            Summary(endpointSummary: summary =>
            {
                summary.Summary = "Endpoint for get artworks viewed history";
                summary.Description = "This endpoint is used for get artworks viewed history.";
                summary.Response<G25HttpResponse>(
                    description: "Represent successful operation response.",
                    example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
                );
            });
        }

        public override async Task<G25HttpResponse> ExecuteAsync(G25Request request, CancellationToken ct)
        {
            var featRequest = new G25Request
            {
                UserId = request.UserId,
                ArtworkType = request.ArtworkType,
                PageNum = request.PageNum,
                ArtNum = request.ArtNum
            };

            var featResponse = await FeatureExtensions.ExecuteAsync<G25Request, G25Response>(
                featRequest,
                ct
            );

            G25HttpResponse httpResponse = G25ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

            if (featResponse.IsSuccess)
            {
                List<G25ResponseDto> g25ResponseDtos = new();
                Collection<G25ChapterDto> g25Chapters = new();

                foreach (var g25Response in featResponse.Result)
                {
                    // Get all chapters viewed of artwork
                    foreach (var g25Chapter in g25Response)
                    {
                        g25Chapters.Add(
                            new G25ChapterDto
                            {
                                Id = g25Chapter.ChapterId,
                                ChapterTitle = g25Chapter.Chapter.Title,
                                ChapterUploadOrder = g25Chapter.Chapter.UploadOrder,
                                ThumbnailUrl = g25Chapter.Chapter.ThumbnailUrl,
                                ViewedAt = g25Chapter.ViewedAt
                            }
                        );
                    }

                    g25ResponseDtos.Add(
                        new G25ResponseDto
                        {
                            ArtworkId = g25Response.First().ArtworkId,
                            artworkType = g25Response.First().ArtworkType,
                            ArtworkTitle = g25Response.First().Artwork.Title,
                            AuthorId = g25Response.First().Artwork.CreatedBy,
                            AuthorName = g25Response.First().Artwork.AuthorName,
                            ThumbnailUrl = g25Response.First().Artwork.ThumbnailUrl,
                            TotalFavorites = g25Response.First().Artwork.ArtworkMetaData.TotalFavorites,
                            TotalStarRates = g25Response.First().Artwork.ArtworkMetaData.TotalStarRates,
                            G25Chapters = g25Chapters
                        }
                    );
                }

                httpResponse.Body = g25ResponseDtos;

                return httpResponse;
            }

            return httpResponse;
        }
    }
}
