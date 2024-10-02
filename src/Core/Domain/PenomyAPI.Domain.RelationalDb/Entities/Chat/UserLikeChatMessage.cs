using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class UserLikeChatMessage : IEntity
{
    public long ChatMessageId { get; set; }

    public long UserId { get; set; }

    public long ValueId { get; set; }

    public DateTime LikedAt { get; set; }

    #region Navigation
    /// <summary>
    ///     The chat message that received this user like (reaction).
    /// </summary>
    public ChatMessage LikedChatMessage { get; set; }

    public UserProfile User { get; set; }

    public UserLikeValue LikeValue { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
