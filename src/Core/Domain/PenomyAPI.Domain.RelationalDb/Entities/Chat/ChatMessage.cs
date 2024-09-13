using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessage : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public long ChatGroupId { get; set; }

    public bool ReplyToAnotherMessage { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public ChatGroup ChatGroup { get; set; }

    public UserProfile Sender { get; set; }

    public ChatMessageReply MessageReceivedReply { get; set; }

    /// <summary>
    ///     Contain the list of all chat messages that reply to this message.
    /// </summary>
    public IEnumerable<ChatMessageReply> RepliedMessages { get; set; }

    public IEnumerable<ChatMessageAttachedMedia> AttachedMedias { get; set; }

    public IEnumerable<UserLikeChatMessage> UserLikes { get; set; }

    public IEnumerable<ChatMessageLikeStatistic> LikeStatistics { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int ContentLength = 2000;
    }
    #endregion
}
