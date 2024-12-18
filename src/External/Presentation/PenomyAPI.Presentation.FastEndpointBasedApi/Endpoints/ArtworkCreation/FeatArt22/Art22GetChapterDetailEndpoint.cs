using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22;

public class Art22GetChapterDetailEndpoint
    : Endpoint<Art22GetChapterDetailRequestDto, Art22GetChapterDetailHttpResponse>
{
    public override void Configure()
    {
        Get("art22/chapter/{chapterId:long}");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art22GetChapterDetailRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art22GetChapterDetailRequestDto>>();
    }

    public override async Task<Art22GetChapterDetailHttpResponse> ExecuteAsync(
        Art22GetChapterDetailRequestDto requestDto,
        CancellationToken ct)
    {
        // Get the state bag contains creatorId extracted from the access-token.
        var stateBag = ProcessorState<StateBag>();

        long creatorId = stateBag.AppRequest.UserId;

        var request = requestDto.MapTo(creatorId);

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art22GetChapterDetailRequest, Art22GetChapterDetailResponse>(
                request,
                ct);

        var httpResponse = Art22GetChapterDetailHttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
