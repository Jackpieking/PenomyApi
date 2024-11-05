using FastEndpoints;
using PenomyAPI.App.FeatG9;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG9;

public sealed class G9Endpoint : Endpoint<G9RequestDto, G9HttpResponse>
{
    public override void Configure()
    {
        Get("g9/comic/{comicId:long}/chapter/{chapterId:long}");

        AllowAnonymous();
    }

    public override async Task<G9HttpResponse> ExecuteAsync(
        G9RequestDto requestDto, CancellationToken cancellationToken)
    {
        var userId = 1;
        var request = requestDto.MapToRequest(userId);

        var featureResponse = await FeatureExtensions.ExecuteAsync<G9Request, G9Response>(
            request,
            cancellationToken);

        var httpResponse = G9HttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, cancellationToken);

        return httpResponse;
    }
}
