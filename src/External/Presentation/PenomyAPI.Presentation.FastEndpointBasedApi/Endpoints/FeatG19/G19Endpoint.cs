using FastEndpoints;
using PenomyAPI.App.FeatG19;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG19;

public class G19Endpoint : Endpoint<G19RequestDto, G19HttpResponse>
{
    public override void Configure()
    {
        Get("g19/anime/{animeId:long}/chapter/{chapterId:long}");

        AllowAnonymous();
    }

    public override async Task<G19HttpResponse> ExecuteAsync(
        G19RequestDto requestDto, CancellationToken ct)
    {
        var userId = 1;
        var request = requestDto.MapToRequest(userId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<G19Request, G19Response>(
            request,
            ct
        );

        var httpResponse = G19HttpResponse.MapFrom(featureResponse);

        return httpResponse;
    }
}
