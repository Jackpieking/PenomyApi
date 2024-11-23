using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM8.DTOs;

public class SM8RequestDto
{
    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public string Description { get; set; }

    [Required]
    public IFormFile CoverPhoto { get; set; }

    public bool RequireApprovedWhenPost { get; set; }
}
