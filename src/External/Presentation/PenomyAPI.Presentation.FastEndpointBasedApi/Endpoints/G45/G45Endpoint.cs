using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G45;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45.HttpResponse;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G45;

public class G45Endpoint : Endpoint<G45RequestDTO, G45HttpResponse>
{
    public override void Configure()
    {
        Get("/g45/followed-artworks");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G45RequestDTO>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get followed artworks";
            summary.Description = "This endpoint is used for user get followed artworks";
            summary.Response<G45HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G45ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G45HttpResponse> ExecuteAsync(
        G45RequestDTO requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G45Request
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = requestDto.ArtworkType,
            ArtNum = G45PaginationOptions.DEFAULT_PAGE_SIZE,
            PageNum = requestDto.PageNum
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G45Request, G45Response>(
            featRequest,
            ct
        );

        var httpResponse = G45ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
