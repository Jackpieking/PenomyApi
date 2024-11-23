using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponseMappers;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12;

public class Art12GetChapterDetailEndpoint
    : Endpoint<Art12GetChapterDetailRequestDto, Art12GetChapterDetailHttpResponse>
{
    public override void Configure()
    {
        Get("art12/chapter/{chapterId:long}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art12GetChapterDetailRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art12GetChapterDetailRequestDto>>();
    }

    public override async Task<Art12GetChapterDetailHttpResponse> ExecuteAsync(
        Art12GetChapterDetailRequestDto requestDto,
        CancellationToken ct)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapTo(creatorId);

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art12GetChapterDetailRequest, Art12GetChapterDetailResponse>(
                request,
                ct);

        var httpResponse = Art12GetChapterDetailHttpResponseMapper.Map(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
