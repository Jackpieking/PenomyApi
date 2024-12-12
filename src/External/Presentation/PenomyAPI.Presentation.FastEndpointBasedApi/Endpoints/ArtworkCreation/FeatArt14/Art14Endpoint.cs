using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt14;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt14.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt14.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt14;

public class Art14Endpoint : Endpoint<Art14RemoveChapterRequestDto, Art14HttpResponse>
{
    public override void Configure()
    {
        Post("art14/remove/chapter");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art14RemoveChapterRequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art14RemoveChapterRequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for getting all the deleted artworks with specified type of current creator account.";
            summary.Description = "This endpoint is used for getting all the deleted artworks with specified type of current creator account.";
            summary.ExampleRequest = new() { ChapterId = 123, };
            summary.Response<Art14HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<Art14HttpResponse> ExecuteAsync(
        Art14RemoveChapterRequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art14Request
        {
            CreatorId = stateBag.AppRequest.UserId,
            ArtworkId = requestDto.ArtworkId,
            ChapterId = requestDto.ChapterId,
        };

        var featResponse = await FeatureExtensions
            .ExecuteAsync<Art14Request, Art14Response>(request, ct);

        var httpResponse = Art14HttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
