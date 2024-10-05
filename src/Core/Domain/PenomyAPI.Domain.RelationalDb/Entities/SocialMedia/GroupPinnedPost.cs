using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPinnedPost : IEntity
{
    public long GroupId { get; set; }

    public long PostId { get; set; }

    public long PinnedBy { get; set; }

    public DateTime PinnedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public GroupPost PinnedGroupPost { get; set; }

    public UserProfile UserWhoPin { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
