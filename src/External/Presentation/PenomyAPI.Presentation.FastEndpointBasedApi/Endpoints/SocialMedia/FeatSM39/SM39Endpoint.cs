using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.SM39;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM39.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM39.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM39;

public class SM39Endpoint : Endpoint<SM39Request, SM39HttpResponse>
{
    public override void Configure()
    {
        Get("sm39/group-member/get");
        AllowAnonymous();
        DontThrowIfValidationFails();
        Description(builder: builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(endpointSummary: summary =>
        {
            summary.Summary = "Endpoint for creating group join request.";
            summary.Description = "This endpoint is used for creating group join request.";
            summary.Response<SM39HttpResponse>(
                description: "Represent successful operation response.",
                example: new() { AppCode = SM39ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<SM39HttpResponse> ExecuteAsync(
        SM39Request request,
        CancellationToken ct
    )
    {
        SM39HttpResponse httpResponse;

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<SM39Request, SM39Response>(
            request,
            ct
        );

        httpResponse = SM39HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new SM39ResponseDto
            {
                Members = featResponse
                    .Members.ToList()
                    .ConvertAll(x => new SM39ResponseObjectDto
                    {
                        UserId = x.Member.UserId.ToString(),
                        NickName = x.Member.NickName,
                        AvatarUrl = x.Member.AvatarUrl,
                        JoinedAt = x.JoinedAt.ToString("dd/MM/yyyy"),
                        LastActiveAt = x.Member.LastActiveAt.ToString("dd/MM/yyyy"),
                        IsManager = x.RoleId == UserRoles.GroupManager.Id,
                    }),
            };
        }
        return httpResponse;
    }
}
