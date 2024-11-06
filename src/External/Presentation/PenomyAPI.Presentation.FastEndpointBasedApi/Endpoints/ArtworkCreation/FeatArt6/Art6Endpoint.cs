using FastEndpoints;
using PenomyAPI.App.FeatArt6;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt6.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt6.HttpResponses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt6;

public class Art6Endpoint : Endpoint<Art6Request, Art6HttpResponse>
{
    public override void Configure()
    {
        Get("art6/comic/chapters/{comicId}");

        AllowAnonymous();
    }

    public override async Task<Art6HttpResponse> ExecuteAsync(Art6Request request, CancellationToken ct)
    {
        var featureResponse = await FeatureExtensions.ExecuteAsync<Art6Request, Art6Response>(
            request,
            ct);

        var httpResponse = new Art6HttpResponse
        {
            Body = featureResponse.Chapters.Select(ComicChapterDto.MapFrom)
        };

        await SendAsync(httpResponse);

        return httpResponse;
    }
}
