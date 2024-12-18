using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt16.OtherHandlers.GetChapters;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16.HttpResponses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt16;

public class Art16GetChaptersEndpoint : Endpoint<Art16GetChaptersRequest, Art16GetChaptersHttpResponse>
{
    public override void Configure()
    {
        Get("art16/anime/chapters/{animeId}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art16GetChaptersRequest>>();
        PreProcessor<ArtworkCreationPreProcessor<Art16GetChaptersRequest>>();
    }

    public override async Task<Art16GetChaptersHttpResponse> ExecuteAsync(
        Art16GetChaptersRequest request,
        CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<Art16GetChaptersRequest, Art16GetChaptersResponse>(
            request,
            ct);

        var httpResponse = new Art16GetChaptersHttpResponse
        {
            Body = featureResponse.Chapters.Select(AnimeChapterItemResponseDto.MapFrom)
        };

        await SendAsync(httpResponse);

        return httpResponse;
    }
}
