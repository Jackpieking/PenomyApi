using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat2.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat2.HttpResponse;
using PenowmyAPI.APP.Chat2;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat2;

public class Chat2Endpoint : Endpoint<EmptyRequest, Chat2HttpResponse>
{
    public override void Configure()
    {
        Get("/Chat2/chat-group/get");

        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);

        PreProcessor<AuthPreProcessor<EmptyRequest>>();

        Description(builder =>
        {
            builder.ClearDefaultProduces(statusCodes: StatusCodes.Status400BadRequest);
        });

        Summary(summary =>
        {
            summary.Summary = "Endpoint for user get created chat groups";
            summary.Description = "This endpoint is used for user get created chat groups";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Chat2HttpResponse
                {
                    AppCode = Chat2ResponseStatusCode.SUCCESS.ToString(),
                }
            );
        });
    }

    public override async Task<Chat2HttpResponse> ExecuteAsync(
        EmptyRequest requestDto,
        CancellationToken ct
    )
    {
        var stateBag = ProcessorState<StateBag>();

        var featRequest = new Chat2Request { UserId = stateBag.AppRequest.UserId };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Chat2Request, Chat2Response>(
            featRequest,
            ct
        );

        var httpResponse = Chat2HttpResponseManager
            .Resolve(featResponse.StatusCode)
            .Invoke(featRequest, featResponse);
        httpResponse.Body = new Chat2ResponseDto();
        httpResponse.Body.Groups = new List<ChatGroupResponseDto>();
        if (featResponse.StatusCode == Chat2ResponseStatusCode.SUCCESS)
            foreach (var p in featResponse.ChatGroups)
            {
                var groupDto = new ChatGroupResponseDto
                {
                    Id = p.Id,
                    GroupName = p.GroupName,
                    IsPublic = p.IsPublic,
                    CoverPhotoUrl = p.CoverPhotoUrl,
                    ChatGroupType = p.ChatGroupType.ToString(),
                    Members = p
                        .ChatGroupMembers.ToList()
                        .Select(x => new ChatGroupMemberResponseDto
                        {
                            MemberId = x.MemberId,
                            RoleId = x.RoleId,
                            JoinedAt = x.JoinedAt,
                            MemberName = x.Member.NickName,
                            AvatarUrl = x.Member.AvatarUrl,
                        }),
                };
                httpResponse.Body.Groups.Add(groupDto);
                await SendAsync(httpResponse, httpResponse.HttpCode, ct);
            }

        return httpResponse;
    }
}
