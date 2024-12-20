using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.Realtime.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub : Hub, IChatHub
{
    public const string connectPath = "/signalr/chat";
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChat2Repository _chat2Repository;

    public ChatHub(IHubContext<ChatHub> hubContext, Lazy<IUnitOfWork> unitOfWork)
    {
        _hubContext = hubContext;
        _chat2Repository = unitOfWork.Value.Chat2Repository;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;
        var groupIds = string.IsNullOrEmpty(userId)
            ? _chat2Repository.GetAllJoinedChatGroupIdAsync(
                    long.Parse(userId),
                    CancellationToken.None
                ).Result
            : null;

        if (groupIds != null)
        {
            foreach (var groupId in groupIds)
            {
                _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;
        var groupIds = string.IsNullOrEmpty(userId)
            ? _chat2Repository.GetAllJoinedChatGroupIdAsync(
                    long.Parse(userId),
                    CancellationToken.None
                ).Result
            : null;

        if (groupIds != null)
        {
            foreach (var groupId in groupIds)
            {
                _hubContext.Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task ReceiveGroupMessange(string groupId, Chat10UserProfileReadModel userChat)
    {
        await _hubContext.Clients.Group(groupId).SendAsync(groupId, userChat);
    }
}
