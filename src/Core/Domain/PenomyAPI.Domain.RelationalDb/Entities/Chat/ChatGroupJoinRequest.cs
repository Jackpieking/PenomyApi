using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatGroupJoinRequest : IEntity, ICreatedEntity<long>
{
    public long ChatGroupId { get; set; }

    public long CreatedBy { get; set; }

    public RequestStatus RequestStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public ChatGroup ChatGroup { get; set; }

    /// <summary>
    ///     The user who create this join request.
    /// </summary>
    public UserProfile Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
