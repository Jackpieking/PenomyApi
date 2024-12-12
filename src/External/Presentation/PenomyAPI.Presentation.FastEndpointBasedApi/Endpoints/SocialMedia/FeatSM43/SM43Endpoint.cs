using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM43;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM43.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM43.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM43;

public class SM43Endpoint : Endpoint<SM43RequestDto, SM43HttpResponse>
{
    public override void Configure()
    {
        Post("sm43/group-join-request/accept");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM43RequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for accepting group join request.";
            summary.Description = "This endpoint is used for accepting group join request.";
            summary.Response<SM43HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM43ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM43HttpResponse> ExecuteAsync(
        SM43RequestDto req,
        CancellationToken ct
    )
    {
        SM43HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM43Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(req.GroupId),
            MemberId = long.Parse(req.MemberId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM43Request, SM43Response>(
            featRequest,
            ct
        );

        httpResponse = SM43HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        return httpResponse;
    }
}
