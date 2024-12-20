using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM12.OtherHandler;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12Other.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12OtherOther.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12OtherOther;

public class SM12OtherEndpoint : Endpoint<EmptyRequest, SM12OtherHttpResponse>
{
    public override void Configure()
    {
        Get("/SM12Other/friend-request/get");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<EmptyRequest>>();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });
        Summary(summary =>
        {
            summary.Summary = "Endpoint for getting friend request";
            summary.Description = "This endpoint is used for getting friend request";
            summary.Response(
                description: "Represent successful operation response.",
                example: new SM12OtherHttpResponse
                {
                    AppCode = Sm12OtherResponseStatusCode.SUCCESS.ToString()
                }
            );
        });
    }

    public override async Task<SM12OtherHttpResponse> ExecuteAsync(
        EmptyRequest requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var sm12Req = new SM12OtherRequest
        {
            UserId = stateBag.AppRequest.UserId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM12OtherRequest, SM12OtherResponse>(
            sm12Req,
            ct
        );

        var httpResponse = SM12OtherHttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);
        return httpResponse;
    }
}
