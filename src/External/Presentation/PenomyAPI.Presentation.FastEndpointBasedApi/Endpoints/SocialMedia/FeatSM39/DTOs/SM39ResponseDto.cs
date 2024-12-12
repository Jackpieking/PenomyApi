using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM39.DTOs;

public class SM39ResponseDto
{
    public List<SM39ResponseObjectDto> Members { get; set; }
}

public class SM39ResponseObjectDto
{
    public string JoinedAt { get; set; }
    public string UserId { get; set; }
    public string NickName { get; set; }
    public string AvatarUrl { get; set; }
    public string LastActiveAt { get; set; }
    public bool IsManager { get; set; }
}
