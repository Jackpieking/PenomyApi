using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PenomyAPI.App.Chat10;
using PenomyAPI.BuildingBlock.FeatRegister.Features;
using PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat10.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat10.HttpResponse;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat10;

public class Chat10Endpoint : Endpoint<Chat10RequestDto, Chat10HttpResponse>
{
    public override void Configure()
    {
        Post("Chat10/chat-groups/get");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        PreProcessor<AuthPreProcessor<Chat10RequestDto>>();

        Summary(summary =>
        {
            summary.Summary = "Endpoint all chat message of chat group.";
            summary.Description = "This endpoint is used for get all chat message of chat group.";
            summary.Response(
                description: "Represent successful operation response.",
                example: new Chat10HttpResponse { AppCode = Chat10ResponseStatusCode.SUCCESS.ToString() }
            );
        });
    }

    public override async Task<Chat10HttpResponse> ExecuteAsync(
        Chat10RequestDto req,
        CancellationToken ct
    )
    {
        Chat10HttpResponse httpResponse;

        var Chat10Request = new Chat10Request
        {
            ChatGroupId = long.Parse(req.GroupChatId),
            ChatNum = req.ChatNum,
            PageNum = req.PageNum
        };

        // Get FeatureHandler response.
        var featResponse = await FeatureExtensions.ExecuteAsync<Chat10Request, Chat10Response>(
            Chat10Request,
            ct
        );

        httpResponse = Chat10HttpResponseManager.Resolve(featResponse.StatusCode).Invoke(featResponse);

        if (featResponse.IsSuccess)
        {
            httpResponse.Body = new Chat10ResponseDto
            {
                UserChats = featResponse.UserChatMessages?.Select(o => new UserChat
                {
                    UserId = o.UserId.ToString(),
                    AvatarUrl = o.AvatarUrl,
                    NickName = o.NickName,
                    Messages = o.Messages.Select(m => new Message
                    {
                        Content = m.Content,
                        Time = m.Time,
                        IsReply = m.IsReply,
                        ReplyMessageId = m.ReplyMessageId.ToString()
                    }).ToList()
                }).ToList()
            };
        }

        await SendAsync(httpResponse, httpResponse.HttpCode, ct);

        return httpResponse;
    }
}
