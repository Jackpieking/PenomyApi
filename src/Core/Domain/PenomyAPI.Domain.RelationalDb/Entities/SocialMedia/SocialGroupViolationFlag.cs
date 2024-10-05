using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroupViolationFlag : EntityWithId<long>
{
    public long GroupId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsResolved { get; set; }

    public string ResolveNote { get; set; }

    public long ResolvedBy { get; set; }

    public DateTime ResolvedAt { get; set; }

    #region Navigation
    public SocialGroup Group { get; set; }

    public SystemAccount Creator { get; set; }

    public UserProfile Resolver { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;

        public const int ResolveNoteLength = 1000;
    }
    #endregion
}
