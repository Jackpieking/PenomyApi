using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.SM12.DTOs;

public class SM12RequestDto
{
    public IEnumerable<IFormFile> AttachedMedia { get; set; }
    public string Title { get; set; }
    public bool AllowComment { get; set; }
    public UserPostPublicLevel PublicLevel { get; set; }
}
