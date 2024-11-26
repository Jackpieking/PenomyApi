using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM41;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM41.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM41.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM41;

public class SM41Endpoint : Endpoint<SM41RequestDto, SM41HttpResponse>
{
    public override void Configure()
    {
        Delete("sm41/group-member/remove/{id}");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM41RequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for removing member group.";
            summary.Description = "This endpoint is used for removing member group.";
            summary.Response<SM41HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM41ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM41HttpResponse> ExecuteAsync(
        SM41RequestDto req,
        CancellationToken ct
    )
    {
        SM41HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM41Request { UserId = stateBag.AppRequest.UserId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM41Request, SM41Response>(
            featRequest,
            ct
        );

        httpResponse = SM41HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        return httpResponse;
    }
}
