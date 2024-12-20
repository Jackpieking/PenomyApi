using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Realtime;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Chat;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using PenomyAPI.Realtime.SignalR.AppConstants.ChatHub;
using PenomyAPI.Realtime.SignalR.Models.ChatHubs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Realtime.SignalR;

[Authorize(AuthenticationSchemes = "Bearer")]
public class ChatHub : Hub, IChatHub
{
    public const string connectPath = "/signalr/chat";
    private readonly IHubContext<ChatHub> _hubContext;
    private IChat2Repository _chat2Repository;
    private IChat3Repository _chat3Repository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly ISnowflakeIdGenerator idGenerator;

    private static readonly IDictionary<long, UserProfile> userProfileDictionary
        = new Dictionary<long, UserProfile>();

    public ChatHub(
        IHubContext<ChatHub> hubContext,
        Lazy<IUnitOfWork> unitOfWork,
        ISnowflakeIdGenerator idGenerator)
    {
        _hubContext = hubContext;
        _unitOfWork = unitOfWork;
        this.idGenerator = idGenerator;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            var userIdValue = long.Parse(userId);

            await AddUserToGroupAsync(userIdValue, CancellationToken.None);
            await AddUserProfileAsync(userIdValue);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User.FindFirst("sub")?.Value ?? string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            var userIdValue = long.Parse(userId);

            await RemoveUserFromGroupHubAsync(userIdValue);
            RemoveUserProfile(userIdValue);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task ReceiveGroupMessange(string groupId, Chat10UserProfileReadModel userChat)
    {
        await _hubContext.Clients.Group(groupName: groupId).SendAsync(groupId, userChat);
    }

    public async Task SendMessage(string groupId, string senderId, string message)
    {
        _chat3Repository = _unitOfWork.Value.Chat3Repository;

        var createdAt = DateTime.UtcNow;

        var chatMessage = new ChatMessage
        {
            Id = idGenerator.Get(),
            ChatGroupId = long.Parse(groupId),
            Content = message,
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
            CreatedBy = long.Parse(senderId),
            MessageType = ChatMessageType.NormalMessage,
            ReplyToAnotherMessage = false,
        };

        var statistic = new ChatMessageLikeStatistic
        {
            ChatMessageId = chatMessage.Id,
            ValueId = 1,
            Total = 0,
        };

        await _chat3Repository.SaveMessageAsync(chatMessage, statistic, CancellationToken.None);

        // Get profile to send back to client.
        var senderIdValue = long.Parse(senderId);

        var profileExisted = userProfileDictionary.ContainsKey(senderIdValue);

        if (!profileExisted)
        {
            userProfileDictionary.TryGetValue(senderIdValue, out var userProfile);

            await Clients
                .Group(groupId)
                .SendAsync(ChatHubClientMethods.ReceiveMessageMethod.MethodName, new SendMessageRequestDto
                {
                    GroupId = groupId,
                    SenderId = senderId,
                    Message = message,
                    AvatarUrl = userProfile.AvatarUrl,
                    NickName = userProfile.NickName,
                    CreatedAt = createdAt,
                });
        }
        else
        {
            await Clients
                .Group(groupId)
                .SendAsync(ChatHubClientMethods.ReceiveMessageMethod.MethodName, new SendMessageRequestDto
                {
                    GroupId = groupId,
                    SenderId = senderId,
                    Message = message,
                    CreatedAt = createdAt
                });
        }
    }

    #region
    private async Task AddUserToGroupAsync(long userId, CancellationToken ct)
    {
        _chat2Repository = _unitOfWork.Value.Chat2Repository;

        var userGroupIds = await _chat2Repository
            .GetAllJoinedChatGroupIdAsync(userId, ct);

        if (!Equals(userGroupIds, null))
        {
            foreach (var groupId in userGroupIds)
            {
                await _hubContext.Groups.AddToGroupAsync(
                    connectionId: Context.ConnectionId,
                    groupName: groupId.ToString());
            }
        }
    }

    private async Task RemoveUserFromGroupHubAsync(long userId)
    {
        _chat2Repository = _unitOfWork.Value.Chat2Repository;

        var userGroupIds = await _chat2Repository
            .GetAllJoinedChatGroupIdAsync(userId, CancellationToken.None);

        if (!Equals(userGroupIds, null))
        {
            foreach (var groupId in userGroupIds)
            {
                await _hubContext.Groups.RemoveFromGroupAsync(
                    connectionId: Context.ConnectionId,
                    groupName: groupId.ToString());
            }
        }
    }

    private async Task AddUserProfileAsync(long userId)
    {
        var profileExisted = userProfileDictionary.ContainsKey(userId);

        if (profileExisted)
        {
            return;
        }

        var userProfile = await _chat2Repository.GetUserProfileByIdAsync(userId);

        if (userProfile != null)
        {
            userProfileDictionary.Add(userId, userProfile);
        }
    }

    private void RemoveUserProfile(long userId)
    {
        var profileExisted = userProfileDictionary.ContainsKey(userId);

        if (profileExisted)
        {
            return;
        }

        userProfileDictionary.Remove(userId);
    }
    #endregion
}
