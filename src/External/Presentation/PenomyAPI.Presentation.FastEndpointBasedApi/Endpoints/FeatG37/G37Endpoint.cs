using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG14;
using PenomyAPI.App.FeatG37;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG37.HttpResponses;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG37;

public class G37Endpoint : Endpoint<EmptyDto, G37HttpResponse>
{
    public override void Configure()
    {
        Post("g37/become-creator");

        PreProcessor<AuthPreProcessor<EmptyDto>>();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for get recommended artworks based on category for users";
            summary.Description = "This endpoint is used for get recommended artworks based on category for users";
            summary.Response(
                description: "Represent successful operation response.",
                example: new G14HttpResponse { AppCode = G14ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G37HttpResponse> ExecuteAsync(EmptyDto empty, CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();

        var request = new G37Request
        {
            UserId = stateBag.AppRequest.UserId,
        };

        var featureResponse = await FeatureExtensions
            .ExecuteAsync<G37Request, G37Response>(request, ct);

        var httpResponse = G37HttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
