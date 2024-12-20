using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatArt3;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt3;

public class Art3Endpoint : Endpoint<Art3RequestDto, Art3HttpResponse>
{
    public override void Configure()
    {
        Get("art3/deleted/artworks");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Art3RequestDto>>();
        PreProcessor<ArtworkCreationPreProcessor<Art3RequestDto>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status403Forbidden);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for getting all the deleted artworks with specified type of current creator account.";
            summary.Description = "This endpoint is used for getting all the deleted artworks with specified type of current creator account.";
            summary.ExampleRequest = new() { ArtworkType = ArtworkType.Comic, };
            summary.Response<Art3HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<Art3HttpResponse> ExecuteAsync(
        Art3RequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new Art3Request
        {
            CreatorId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<Art3Request, Art3Response>(request, ct);

        var httpResponse = Art3HttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
