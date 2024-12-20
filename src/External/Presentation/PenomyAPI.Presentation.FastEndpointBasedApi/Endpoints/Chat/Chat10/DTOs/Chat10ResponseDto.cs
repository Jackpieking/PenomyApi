using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat10.DTOs;

public class Chat10ResponseDto
{
    public ICollection<UserChat> UserChats { get; set; }
}

public class UserChat
{
    public string UserId { get; set; }
    public string AvatarUrl { get; set; }
    public string NickName { get; set; }
    public ICollection<Message> Messages { get; set; }
}

public class Message
{
    public string Content { get; set; }
    public DateTime Time { get; set; }
    public bool IsReply { get; set; }
    public string ReplyMessageId { get; set; }
}
