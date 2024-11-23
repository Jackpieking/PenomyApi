using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Profile.DTOs;

public class SM38ProfileRequestDto
{
    public string GroupId { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public bool RequireApprovedWhenPost { get; set; }
}
