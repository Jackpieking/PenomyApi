namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM5.DTOs;

public class SM5ResponseDto
{
    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public string Description { get; set; }

    public string CoverPhotoUrl { get; set; }

    public int TotalMembers { get; set; }

    public string CreatedAt { get; set; }

    public bool RequireApprovedWhenPost { get; set; }

    public string ManagerName { get; set; }

    public bool IsManager { get; set; }
}
