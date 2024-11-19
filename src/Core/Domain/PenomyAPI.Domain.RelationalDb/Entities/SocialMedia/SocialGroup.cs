using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class SocialGroup : EntityWithId<long>, ICreatedEntity<long>
{
    public string Name { get; set; }

    public bool IsPublic { get; set; }

    public string Description { get; set; }

    public string CoverPhotoUrl { get; set; }

    public int TotalMembers { get; set; }

    public bool RequireApprovedWhenPost { get; set; }

    public SocialGroupStatus GroupStatus { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public UserProfile Creator { get; set; }

    public IEnumerable<SocialGroupMember> GroupMembers { get; set; }

    public IEnumerable<SocialGroupJoinRequest> SocialGroupJoinRequests { get; set; }

    public SocialGroupLinkedChatGroup SocialGroupLinkedChatGroup { get; set; }

    public IEnumerable<GroupPinnedPost> GroupPinnedPosts { get; set; }

    public IEnumerable<GroupPost> GroupPosts { get; set; }

    public IEnumerable<SocialGroupReport> ReceivedSocialGroupReports { get; set; }

    public IEnumerable<SocialGroupRelatedArtwork> SocialGroupRelatedArtworks { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NameLength = 200;

        public const int DescriptionLength = 2000;

        public const int CoverPhotoUrlLength = 2000;
    }
    #endregion
}
