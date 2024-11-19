using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.SM14;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM14;

public class SM14Endpoint : Endpoint<SM14RequestDto, SM14HttpResponse>
{
    public override void Configure()
    {
        Delete("sm14/remove/{postId}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM14RequestDto>>();
    }

    public override async Task<SM14HttpResponse> ExecuteAsync(
        SM14RequestDto requestDto,
        CancellationToken ct)
    {
        var stateBag = ProcessorState<StateBag>();
        var userId = stateBag.AppRequest.UserId;
        var request = new SM14Request
        {
            UserId = userId,
            PostId = requestDto.PostId
        };
        var featureResponse = await FeatureExtensions.ExecuteAsync<SM14Request, SM14Response>(
            request,
            ct);

        var t = HttpContext;

        var httpResponse = SM14HttpResponseManager
            .Resolve(featureResponse.AppCode)
            .Invoke(featureResponse);

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
