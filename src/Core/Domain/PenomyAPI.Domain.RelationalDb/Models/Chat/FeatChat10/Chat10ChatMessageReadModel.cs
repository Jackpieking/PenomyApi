using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Chat.FeatChat10;

public class Chat10ChatMessageReadModel
{
    public long ChatId { get; set; }
    public string Content { get; set; }
    public DateTime Time { get; set; }
    public bool IsReply { get; set; }
    public long ReplyMessageId { get; set; }
}
