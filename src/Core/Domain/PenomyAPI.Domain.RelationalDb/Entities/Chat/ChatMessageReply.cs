using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessageReply : IEntity
{
    /// <summary>
    ///     The id of the chat message that reply to another message.
    /// </summary>
    public long RootChatMessageId { get; set; }

    /// <summary>
    ///     The id of the chat message that's replied by some messages.
    /// </summary>
    public long RepliedMessageId { get; set; }

    #region Navigation
    public ChatMessage RootChatMessage { get; set; }

    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
