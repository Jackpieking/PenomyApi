using System;

namespace PenomyAPI.Realtime.SignalR.Models.ChatHubs;

public sealed class SendMessageRequestDto
{
    public string GroupId { get; set; }

    public string SenderId { get; set; }

    public string Message { get; set; }

    public string AvatarUrl { get; set; }

    public string NickName { get; set; }

    public DateTime CreatedAt { get; set; }
}
