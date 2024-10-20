using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.DTOs;

public sealed class G1RequestDto
{
    [Required]
    public string Email { get; set; }
}
