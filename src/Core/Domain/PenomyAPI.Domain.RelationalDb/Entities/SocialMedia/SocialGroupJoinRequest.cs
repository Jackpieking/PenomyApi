using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroupJoinRequest : IEntity, ICreatedEntity<long>
{
    public long GroupId { get; set; }

    public long CreatedBy { get; set; }

    public RequestStatus RequestStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    /// <summary>
    ///     The user who creates this join request.
    /// </summary>
    public UserProfile Creator { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
