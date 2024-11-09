using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G63.DTOs;

public class G63ResponseDto
{
    public string CreatorId { get; set; }
    public string NickName { get; set; }
    public string AvatarUrl { get; set; }
    public UserGender Gender { get; set; }
    public string AboutMe { get; set; }
    public int TotalArtworks { get; set; }
    public int TotalFollowers { get; set; }

}
