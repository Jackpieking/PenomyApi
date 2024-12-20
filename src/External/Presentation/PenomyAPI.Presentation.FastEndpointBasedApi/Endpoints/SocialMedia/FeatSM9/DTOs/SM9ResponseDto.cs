using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM9.DTOs;

public class SM9ResponseDto
{
    public List<SM9ResponseObjectDto> GroupList { get; set; }
}

public class SM9ResponseObjectDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int TotalMembers { get; set; }
    public string CoverImgUrl { get; set; }
    public string CreatedAt { get; set; }
    public long TotalPosts { get; set; }
}
