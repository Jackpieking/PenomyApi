using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G48;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48;

public class G48Endpoint : Endpoint<G48RequestDTOs, G48HttpResponse>
{
    public override void Configure()
    {
        Get("/G48/favorite-artworks");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G48RequestDTOs>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get favorite artworks";
            summary.Description = "This endpoint is used for user get favorite artworks";
            summary.Response<G48HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G48ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G48HttpResponse> ExecuteAsync(
        G48RequestDTOs requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G48Request
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
            ArtNum = G48PaginationOptions.DEFAULT_PAGE_SIZE,
            PageNum = requestDto.PageNum
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G48Request, G48Response>(
            featRequest,
            ct
        );

        var httpResponse = G48ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
