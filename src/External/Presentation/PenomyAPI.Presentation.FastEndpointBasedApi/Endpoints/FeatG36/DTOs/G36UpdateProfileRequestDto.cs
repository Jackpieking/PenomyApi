using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG36.DTOs;

public class G36UpdateProfileRequestDto
{
    [Required]
    public string NickName { get; set; }

    [Required]
    public string AboutMe { get; set; }

    public IFormFile AvatarFile { get; set; }

    public bool HasUploadAvatarFile() => AvatarFile != null;
}
