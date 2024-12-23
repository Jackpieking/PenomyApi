using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessage : EntityWithId<long>, ICreatedEntity<long>
{
    public string Content { get; set; }

    public ChatMessageType MessageType { get; set; } = ChatMessageType.NormalMessage;

    public long ChatGroupId { get; set; }

    public bool ReplyToAnotherMessage { get; set; }

    public bool IsRevoked { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public ChatGroup ChatGroup { get; set; }

    public UserProfile Sender { get; set; }

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
