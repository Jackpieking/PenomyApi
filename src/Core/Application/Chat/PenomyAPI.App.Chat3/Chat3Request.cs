using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;

namespace PenomyAPI.App.Chat3;

public class Chat3Request : IFeatureRequest<Chat3Response>
{
    public string Content { get; init; }

    public ChatMessageType MessageType { get; init; } = ChatMessageType.NormalMessage;
    public long ChatGroupId { get; init; }
    public long MessageId { get; init; }
    public long UserId { get; init; }
    public bool IsReply { get; init; }
}
