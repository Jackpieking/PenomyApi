using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM42;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM42.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM42.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM42;

public class SM42Endpoint : Endpoint<SM42Request, SM42HttpResponse>
{
    public override void Configure()
    {
        Get("sm42/group-join-request/get");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM42Request>>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting group join requests.";
            summary.Description = "This endpoint is used for getting group join requests.";
            summary.Response<SM42HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM42ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM42HttpResponse> ExecuteAsync(SM42Request req, CancellationToken ct)
    {
        SM42HttpResponse httpResponse;

        var stateBag = ProcessorState<StateBag>();

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM42Request, SM42Response>(req, ct);

        httpResponse = SM42HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM42ResponseDto
            {
                RequestList = featResponse.RequestList.ConvertAll(x => new SM42ResponseObjectDto
                {
                    UserId = x.Creator.UserId.ToString(),
                    UserName = x.Creator.NickName,
                    UserAvatar = x.Creator.AvatarUrl,
                    CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                }),
            };

            return httpResponse;
        }
        return httpResponse;
    }
}
