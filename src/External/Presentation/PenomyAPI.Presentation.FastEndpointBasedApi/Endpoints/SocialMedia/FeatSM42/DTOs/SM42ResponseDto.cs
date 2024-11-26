using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM42.DTOs;

public class SM42ResponseDto
{
    public List<SM42ResponseObjectDto> RequestList { get; set; }
}

public class SM42ResponseObjectDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserAvatar { get; set; }
    public string CreatedAt { get; set; }
}
