using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM15;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM15.HttpResponse;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM15;

public class Sm15Endpoint : Endpoint<SM15RequestDto, Sm15HttpResponse>
{
    public override void Configure()
    {
        Get("/SM15/created-post/get");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<SM15RequestDto>>();

        Description(builder => { builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest); });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get created post";
            summary.Description = "This endpoint is used for user get created post";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Sm15HttpResponse { AppCode = SM15ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Sm15HttpResponse> ExecuteAsync(
        SM15RequestDto requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new SM15Request
        {
            UserId = stateBag.AppRequest.UserId
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM15Request, SM15Response>(
            featRequest,
            ct
        );

        var httpResponse = SM15ResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);

        if (featResponse.StatusCode == SM15ResponseStatusCode.SUCCESS)
            featResponse.UserPosts.ForEach(p => httpResponse.Body.UserPosts.Add(new UserPostDto
            {
                Id = p.Id,
                Content = p.Content,
                CreatedBy = p.Creator.NickName,
                CreatedAt = p.CreatedAt,
                AllowComment = p.AllowComment,
                PublicLevel = p.PublicLevel,
                TotalLikes = p.TotalLikes,
                AttachedMedias = p.AttachedMedias.Select(m => new AttachMediaDto
                {
                    FileName = m.FileName,
                    MediaType = m.MediaType,
                    StorageUrl = m.StorageUrl,
                    UploadOrder = m.UploadOrder
                }).ToList()
            }));

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
