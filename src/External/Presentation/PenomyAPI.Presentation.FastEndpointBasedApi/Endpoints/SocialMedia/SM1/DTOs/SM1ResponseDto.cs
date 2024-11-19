using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM1.DTOs;

public class SM1ResponseDto
{
    public string NickName { get; set; }
    public UserGender Gender { get; set; }
    public string AvatarUrl { get; set; }
    public string AboutMe { get; set; }
    public int TotalFollowedCreators { get; set; }
    public DateTime RegisteredAt { get; set; }
}
