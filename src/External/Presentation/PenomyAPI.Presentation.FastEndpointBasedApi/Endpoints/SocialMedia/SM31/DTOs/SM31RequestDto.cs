using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.SM31.DTOs;

public class SM31RequestDto
{
    [Required]
    public string FriendId { get; set; }
}
