using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG35;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.HttpResponses;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35.PreProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG35;

public sealed class G35Endpoint : EndpointWithoutRequest<G35HttpResponse>
{
    public override void Configure()
    {
        Get("g35/user/profile");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<G35AuthorizationPreProcessor>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(StatusCodes.Status401Unauthorized);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for geting user profile";
            summary.Description = "This endpoint is used for getting the user profile belonged to input userId.";
            summary.ExampleRequest = new() { };
            summary.Response<G35HttpResponse>(
                description: "Represent successful operation response.",
                example: new()
                {
                    HttpCode = StatusCodes.Status200OK,
                }
            );
        });
    }

    public override async Task<G35HttpResponse> ExecuteAsync(CancellationToken ct)
    {
        var preprocessState = ProcessorState<G35StateBag>();

        // If the request is unauthorized then not processed.
        if (!preprocessState.IsAuthorized)
        {
            var unauthorizedHttpResponse = G35HttpResponse.UNAUTHORIZED();

            await SendAsync(
                unauthorizedHttpResponse,
                unauthorizedHttpResponse.HttpCode,
                ct);

            return unauthorizedHttpResponse;
        }

        // Process the request when valid.
        var request = new G35Request()
        {
            UserId = preprocessState.UserId
        };

        var featureResponse = await FeatureExtensions.ExecuteAsync<G35Request, G35Response>(
            request,
            ct);

        var httpResponse = G35HttpResponse.MapFrom(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
