using System.ComponentModel.DataAnnotations.Schema;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessageLikeStatistic : IEntity
{
    public long ChatMessageId { get; set; }

    public long ValueId { get; set; }

    /// <summary>
    ///     The total of a specific user like value.
    /// </summary>
    public long Total { get; set; }

    public static ChatMessageLikeStatistic Empty(long chatId)
    {
        return new ChatMessageLikeStatistic { ChatMessageId = chatId, ValueId = 0, Total = 0 };
    }

    #region MetaData

    public static class MetaData
    {
    }

    #endregion

    #region Navigation

    public ChatMessage ChatMessage { get; set; }

    // [NotMapped]
    // public UserLikeValue LikeValue { get; set; }
    #endregion
}
