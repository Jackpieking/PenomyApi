using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM44;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM44.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM44.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM44;

public class SM44Endpoint : Endpoint<SM44RequestDto, SM44HttpResponse>
{
    public override void Configure()
    {
        Post("sm44/group-join-request/create");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM44RequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating group join request.";
            summary.Description = "This endpoint is used for creating group join request.";
            summary.Response<SM44HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM44ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM44HttpResponse> ExecuteAsync(
        SM44RequestDto req,
        CancellationToken ct
    )
    {
        SM44HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM44Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(req.GroupId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM44Request, SM44Response>(
            featRequest,
            ct
        );

        httpResponse = SM44HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        httpResponse.Body = featResponse;
        return httpResponse;
    }
}
