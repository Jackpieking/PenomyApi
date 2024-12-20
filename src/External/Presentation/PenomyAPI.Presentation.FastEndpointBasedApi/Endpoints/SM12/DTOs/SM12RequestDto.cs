using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.DTOs;

public class SM12RequestDto
{
    public IEnumerable<IFormFile> AttachedMedia { get; set; }

    [Required] public string Title { get; set; }

    [Required] public bool AllowComment { get; set; }

    [Required] public UserPostPublicLevel PublicLevel { get; set; }
}
