using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroupMember : IEntity
{
    public long GroupId { get; set; }

    public long MemberId { get; set; }

    public long RoleId { get; set; }

    public DateTime JoinedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public UserProfile Member { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
