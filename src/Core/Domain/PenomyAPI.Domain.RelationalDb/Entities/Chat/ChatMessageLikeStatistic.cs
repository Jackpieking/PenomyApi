using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessageLikeStatistic : IEntity
{
    public long ChatMessageId { get; set; }

    public UserLikeChatMessageValue Value { get; set; }

    /// <summary>
    ///     The total of a specific user like value.
    /// </summary>
    public long Total { get; set; }

    #region Navigation
    public ChatMessage ChatMessage { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
