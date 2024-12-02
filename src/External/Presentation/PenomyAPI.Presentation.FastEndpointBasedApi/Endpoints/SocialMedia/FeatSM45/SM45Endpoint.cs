using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM45;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM45.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM45;

public class SM45Endpoint : Endpoint<SM45Request, SM45HttpResponse>
{
    public override void Configure()
    {
        Delete("sm45/group-join-request/delete");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM45Request>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for deleting group join request.";
            summary.Description = "This endpoint is used for deleting group join request.";
            summary.Response<SM45HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM45ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM45HttpResponse> ExecuteAsync(SM45Request req, CancellationToken ct)
    {
        SM45HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        req.SetUserId(stateBag.AppRequest.UserId.ToString());
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM45Request, SM45Response>(req, ct);

        httpResponse = SM45HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        httpResponse.Body = featResponse;
        return httpResponse;
    }
}
