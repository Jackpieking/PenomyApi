using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserFriendRequest : IEntity
{
    public long FriendId { get; set; }

    public long CreatedBy { get; set; }

    public RequestStatus RequestStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region MetaData
    public static class MetaData { }
    #endregion
}
