using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SocialMedia.FeatSM30.DTOs;

public class SM30RequestDto
{
    [Required] public long FriendId { get; set; }
}
