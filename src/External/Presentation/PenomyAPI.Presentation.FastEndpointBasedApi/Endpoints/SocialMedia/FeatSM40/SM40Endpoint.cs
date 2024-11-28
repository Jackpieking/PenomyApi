using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM40;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM40.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM40.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM40;

public class SM40Endpoint : Endpoint<SM40RequestDto, SM40HttpResponse>
{
    public override void Configure()
    {
        Post("sm40/group-member/assign-role");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM40RequestDto>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for changing member group role.";
            summary.Description = "This endpoint is used for changing member group role.";
            summary.Response<SM40HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM40ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM40HttpResponse> ExecuteAsync(
        SM40RequestDto req,
        CancellationToken ct
    )
    {
        SM40HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM40Request
        {
            UserId = stateBag.AppRequest.UserId,
            GroupId = long.Parse(req.GroupId),
            MemberId = long.Parse(req.MemberId),
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM40Request, SM40Response>(
            featRequest,
            ct
        );

        httpResponse = SM40HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);
        httpResponse.Body = featResponse;
        return httpResponse;
    }
}
