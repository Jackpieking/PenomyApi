using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat2.DTOs;

public class Chat2ResponseDto
{
    public List<ChatGroupResponseDto> Groups { get; set; }
}

public class ChatGroupResponseDto
{
    public long Id { get; set; }
    public string GroupName { get; set; }
    public bool IsPublic { get; set; }
    public string CoverPhotoUrl { get; set; }
    public string ChatGroupType { get; set; }
    public IEnumerable<ChatGroupMemberResponseDto> Members { get; set; } = [];
}

public class ChatGroupMemberResponseDto
{
    public long MemberId { get; set; }
    public string MemberName { get; set; }
    public long RoleId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string AvatarUrl { get; set; }
}
