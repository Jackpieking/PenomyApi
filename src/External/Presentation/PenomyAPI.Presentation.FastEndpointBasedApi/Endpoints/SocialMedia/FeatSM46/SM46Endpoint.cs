using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM46;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM46.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM46;

public class SM46Endpoint : Endpoint<SM46Request, SM46HttpResponse>
{
    public override void Configure()
    {
        Delete("sm46/group-join-request/reject");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM46Request>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for rejecting group join request.";
            summary.Description = "This endpoint is used for rejecting group join request.";
            summary.Response<SM46HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM46ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM46HttpResponse> ExecuteAsync(SM46Request req, CancellationToken ct)
    {
        SM46HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        req.SetUserId(stateBag.AppRequest.UserId.ToString());
        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM46Request, SM46Response>(req, ct);

        httpResponse = SM46HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        httpResponse.Body = featResponse;
        return httpResponse;
    }
}
