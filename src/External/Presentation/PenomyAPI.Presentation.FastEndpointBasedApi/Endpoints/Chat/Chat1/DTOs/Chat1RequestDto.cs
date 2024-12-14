using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat1.DTOs;

public class Chat1RequestDto
{
    [Required] public string GroupName { get; set; }
    public bool IsPublic { get; set; }
    [Required] public IFormFile CoverPhoto { get; set; }
    public ChatGroupType GroupType { get; set; }
}
