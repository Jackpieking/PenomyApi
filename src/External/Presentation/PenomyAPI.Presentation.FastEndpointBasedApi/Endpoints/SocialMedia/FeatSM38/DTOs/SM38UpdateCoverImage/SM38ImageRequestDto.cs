using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM38Image.DTOs;

public class SM38ImageRequestDto
{
    [Required]
    public string GroupId { get; set; }
    [Required]
    public IFormFile CoverPhoto { get; set; }

}
