using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25;

public class G25Endpoint : Endpoint<G25RequestDto, G25HttpResponse>
{
    public override void Configure()
    {
        Get("g25/user/view-history");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G25RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for get artworks viewed history";
            summary.Description = "This endpoint is used for get artworks viewed history.";
            summary.Response<G25HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G25ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G25HttpResponse> ExecuteAsync(
        G25RequestDto request,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G25Request
        {
            UserId = stateBag.AppRequest.UserId,
            ArtworkType = request.ArtworkType,
            PageNum = request.PageNum,
            ArtNum = G48PaginationOptions.DEFAULT_PAGE_SIZE
        };

        var featResponse = await FeatureExtensions.ExecuteAsync<G25Request, G25Response>(
            featRequest,
            ct
        );

        G25HttpResponse httpResponse = G25ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
