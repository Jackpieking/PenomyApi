using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32.DTOs;

public class SM32ResponseDto
{
    public IEnumerable<UserResponseDto> Users { get; set; }
    public IEnumerable<UserResponseDto> FriendLists { get; set; }
}

public class UserResponseDto
{
    public string UserId { get; set; }
    public string NickName { get; set; }
    public string AvatarUrl { get; set; }
    public UserGender Gender { get; set; }
    public string AboutMe { get; set; }
    public bool IsFriend { get; set; }
    public string ChatGroupId { get; set; }
    public bool HasSentToMeFriendRequest { get; set; }
    public bool HasSentByMeFriendRequest { get; set; }
}
