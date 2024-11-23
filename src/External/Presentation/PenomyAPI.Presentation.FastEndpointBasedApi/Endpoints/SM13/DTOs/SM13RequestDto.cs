using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM13.DTOs;

public class SM13RequestDto
{
    [Required] public long PostId { get; set; }

    [Required] public string Content { get; set; }

    public IEnumerable<IFormFile> AttachedMedia { get; set; }

    [Required] public bool AllowComment { get; set; }

    [Required] public UserPostPublicLevel PublicLevel { get; set; }

    public bool IsAttachedMediaUpdated()
    {
        return AttachedMedia is not null;
    }
}
