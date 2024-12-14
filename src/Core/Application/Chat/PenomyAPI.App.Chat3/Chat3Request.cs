using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.App.Chat3;

public class Chat3Request : IFeatureRequest<Chat3Response>
{
    public string Content { get; set; }

    public ChatMessageType MessageType { get; set; } = ChatMessageType.NormalMessage;
    public long ChatGroupId { get; set; }
    public long MessageId { get; set; }
    public long UserId { get; set; }
    public bool IsReply { get; set; }
}
