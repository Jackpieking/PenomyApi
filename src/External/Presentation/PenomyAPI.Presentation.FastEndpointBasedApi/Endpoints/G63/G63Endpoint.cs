using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G63;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63;

public class G63Endpoint : Endpoint<G63RequestDto, G63HttpResponse>
{
    public override void Configure()
    {
        Get("/G63/follow-creator/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<G63RequestDto>>();

        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for user get favorite artworks";
            summary.Description = "This endpoint is used for user get favorite artworks";
            summary.Response<G63HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = G63ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<G63HttpResponse> ExecuteAsync(
        G63RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new G63Request
        {
            UserId = stateBag.AppRequest.UserId,
            PageNum = requestDto.PageNum,
            CreatorNum = G63PaginationOptions.DEFAULT_PAGE_SIZE

        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<G63Request, G63Response>(
            featRequest,
            ct
        );

        var httpResponse = G63ResponseManager
                .Resolve(featResponse.StatusCode)
                .Invoke(featRequest, featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = featResponse.Result.Select(o =>
                new G63ResponseDto
                {
                    CreatorId = o.CreatorId.ToString(),
                    NickName = o.ProfileOwner.NickName,
                    AvatarUrl = o.ProfileOwner.AvatarUrl,
                    Gender = o.ProfileOwner.Gender,
                    AboutMe = o.ProfileOwner.AboutMe,
                    TotalArtworks = o.TotalArtworks,
                    TotalFollowers = o.TotalFollowers,
                }).ToList();
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
