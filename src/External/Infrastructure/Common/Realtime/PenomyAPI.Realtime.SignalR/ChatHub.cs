using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.Realtime.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub : Hub
{
    public const string connectPath = "/signalr/chat";
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IG63Repository _g63Repository;

    public ChatHub(IHubContext<ChatHub> hubContext, Lazy<IUnitOfWork> unitOfWork)
    {
        _hubContext = hubContext;
        _g63Repository = unitOfWork.Value.G63Repository;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;
        var groupIds = string.IsNullOrEmpty(userId)
            ? _g63Repository.GetAllJoinedChatGroupIdStringAsync(
                long.Parse(userId)
                ).Result
            : null;

        if (groupIds != null)
        {
            foreach (var groupId in groupIds)
            {
                _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;
        var groupIds = string.IsNullOrEmpty(userId)
            ? _g63Repository.GetAllJoinedChatGroupIdStringAsync(
                long.Parse(userId)
                ).Result
            : null;

        if (groupIds != null)
        {
            foreach (var groupId in groupIds)
            {
                _hubContext.Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessange(string groupId, string name, string imageURL, string message)
    {
        await _hubContext.Clients.Group(groupId).SendAsync(groupId, name, imageURL, message);
    }
}
