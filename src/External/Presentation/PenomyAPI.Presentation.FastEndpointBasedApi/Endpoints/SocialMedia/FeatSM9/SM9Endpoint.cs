using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM9;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.Middlewares.Authorization;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9;

public class SM9Endpoint : Endpoint<SM9RequestDto, SM9HttpResponse>
{
    private static readonly IFormFileHelper _formFileHelper;

    static SM9Endpoint()
    {
        _formFileHelper = FormFileHelper.Instance;
    }

    public override void Configure()
    {
        Get("sm9/created-groups/get");
        DontThrowIfValidationFails();
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<SM9AuthorizationPreProcessor>();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for getting created groups.";
            summary.Description = "This endpoint is used for getting created groups.";
            summary.Response<SM9HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM9ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM9HttpResponse> ExecuteAsync(
        SM9RequestDto req,
        CancellationToken ct
    )
    {
        SM9HttpResponse httpResponse;

        var stateBag = ProcessorState<SM9StateBag>();

        var featRequest = new SM9Request
        {
            UserId = stateBag.AppRequest.GetUserId(),
            MaxRecord = req.MaxRecord,
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM9Request, SM9Response>(
            featRequest,
            ct
        );

        httpResponse = SM9HttpResponseManager.Resolve(featResponse.StatusCode).Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM9ResponseDto
            {
                GroupList = featResponse.Result.ConvertAll(x => new SM9ResponseObjectDto
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    TotalMembers = x.TotalMembers,
                    CoverImgUrl = x.CoverPhotoUrl,
                    CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy"),
                    TotalPosts = x.GroupPosts.Count(),
                }),
            };

            return httpResponse;
        }

        return httpResponse;
    }
}
