using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.App.G25.OtherHandlers.RemoveUserHistoryItem;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25RemoveUserHistoryEndpoint
    : Endpoint<G25RemoveUserHistoryRequestDto, G25RemoveUserHistoryHttpResponse>
{
    public override void Configure()
    {
        Post("g25/user/remove-history");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<G25RemoveUserHistoryRequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for removing one user's view history item.";
            summary.Description = "This endpoint is used for removing one user's view history item.";
            summary.Response<G25HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G25RemoveUserHistoryHttpResponse> ExecuteAsync(
        G25RemoveUserHistoryRequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G25RemoveUserHistoryItemRequest
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkId = requestDto.ArtworkId,
        };

        var featResponse = await FeatureExtensions
            .ExecuteAsync<G25RemoveUserHistoryItemRequest, G25RemoveUserHistoryItemReponse>(featRequest, ct);

        var httpResponse = G25RemoveUserHistoryHttpResponse.MapFrom(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
