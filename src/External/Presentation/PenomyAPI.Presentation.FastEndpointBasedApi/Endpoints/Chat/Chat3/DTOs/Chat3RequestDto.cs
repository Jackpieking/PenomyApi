using System.ComponentModel.DataAnnotations;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Chat.Chat3.DTOs;

public class Chat3RequestDto
{
    [Required] public string Content { get; set; }

    [Required] public ChatMessageType MessageType { get; set; }

    public string ChatGroupId { get; set; }
    public bool IsReply { get; set; }
}
