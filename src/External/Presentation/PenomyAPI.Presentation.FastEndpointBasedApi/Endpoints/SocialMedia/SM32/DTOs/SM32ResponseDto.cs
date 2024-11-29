using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM32.DTOs;

public class SM32ResponseDto
{
    public IEnumerable<UserResponseDto> Users { get; set; }
}

public class UserResponseDto
{
    public long UserId { get; set; }
    public string NickName { get; set; }
    public string AvatarUrl { get; set; }
    public UserGender Gender { get; set; }
    public string AboutMe { get; set; }
}
