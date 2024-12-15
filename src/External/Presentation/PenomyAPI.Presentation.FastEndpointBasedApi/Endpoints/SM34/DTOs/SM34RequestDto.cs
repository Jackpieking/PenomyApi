using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM34.DTOs;

public class SM34RequestDto
{
    public IEnumerable<IFormFile> AttachedMedia { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public bool AllowComment { get; set; }

    [Required]
    public string GroupId { get; set; }
}
