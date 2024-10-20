using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Chat;
using PenomyAPI.Domain.RelationalDb.Entities.Monetization;
using PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;
using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class UserProfile : IEntity
{
    public long UserId { get; set; }

    public string NickName { get; set; }

    public UserGender Gender { get; set; }

    public string AvatarUrl { get; set; }

    public string AboutMe { get; set; }

    public bool RegisterAsCreator { get; set; }

    public int TotalFollowedCreators { get; set; }

    public DateTime LastActiveAt { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    #region Navigation
    // Generic domain
    public CreatorProfile CreatorProfile { get; set; }

    public IEnumerable<UserFollowedCreator> FollowedCreators { get; set; }

    public IEnumerable<UserFollowedCreator> Followers { get; set; }

    public IEnumerable<BugReport> CreatedBugReports { get; set; }

    public IEnumerable<UserProfileReport> CreatedUserReports { get; set; }

    public IEnumerable<UserProfileReport> ReceivedUserReports { get; set; }

    public IEnumerable<UserBan> UserBans { get; set; }

    public IEnumerable<GrantedAuthorizedUser> GrantedTickets { get; set; }

    // Chat domain
    public IEnumerable<ChatGroup> CreatedChatGroups { get; set; }

    public IEnumerable<ChatGroupMember> JoinedChatGroupMembers { get; set; }

    public IEnumerable<ChatGroupJoinRequest> ChatGroupJoinRequests { get; set; }

    public IEnumerable<ChatMessage> ChatMessages { get; set; }

    public IEnumerable<UserLikeChatMessage> UserLikeChatMessages { get; set; }

    // Social media domain
    public IEnumerable<UserChatGroupActiveHistory> ChatGroupActiveHistories { get; set; }

    public IEnumerable<UserLikeUserPostComment> UserLikeUserPostComments { get; set; }

    public IEnumerable<SocialGroup> CreatedSocialGroups { get; set; }

    public IEnumerable<SocialGroupMember> JoinedSocialGroupMembers { get; set; }

    public IEnumerable<SocialGroupJoinRequest> SocialGroupJoinRequests { get; set; }

    public IEnumerable<GroupPinnedPost> GroupPinnedPosts { get; set; }

    public IEnumerable<GroupPost> CreatedGroupPosts { get; set; }

    public IEnumerable<GroupPost> ApprovedGroupPosts { get; set; }

    public IEnumerable<UserLikeGroupPost> UserLikeGroupPosts { get; set; }

    public IEnumerable<UserLikeGroupPostComment> UserLikeGroupPostComments { get; set; }

    public IEnumerable<GroupPostComment> CreatedGroupPostComments { get; set; }

    public IEnumerable<GroupPostReport> CreatedGroupPostReports { get; set; }

    public IEnumerable<UserPostReport> CreatedUserPostReports { get; set; }

    public IEnumerable<UserSavedUserPost> SavedUserPosts { get; set; }

    public IEnumerable<UserSavedGroupPost> SavedGroupPosts { get; set; }

    public IEnumerable<UserPost> CreatedUserPosts { get; set; }

    public IEnumerable<UserLikeUserPost> UserLikeUserPosts { get; set; }

    public IEnumerable<UserPostComment> CreatedUserPostComments { get; set; }

    public IEnumerable<SocialGroupReport> CreatedSocialGroupReports { get; set; }

    public IEnumerable<SocialGroupViolationFlag> ResolvedSocialGroupViolationFlags { get; set; }

    // Artwork creation domain
    public IEnumerable<Artwork> CreatedArtworks { get; set; }

    public IEnumerable<Artwork> UpdatedArtworks { get; set; }

    public IEnumerable<Artwork> TemporarilyRemovedArtworks { get; set; }

    public IEnumerable<ArtworkChapter> CreatedChapters { get; set; }

    public IEnumerable<ArtworkChapter> UpdatedChapters { get; set; }

    public IEnumerable<ArtworkChapter> TemporarilyRemovedChapters { get; set; }

    public IEnumerable<Series> CreatedSeries { get; set; }

    public IEnumerable<Series> UpdatedSeries { get; set; }

    public IEnumerable<Series> TemporarilyRemovedSeries { get; set; }

    public IEnumerable<ArtworkComment> CreatedArtworkComments { get; set; }

    public IEnumerable<ArtworkReport> CreatedArtworkReports { get; set; }

    public IEnumerable<ArtworkChapterReport> CreatedChapterReports { get; set; }

    public IEnumerable<ArtworkBugReport> CreatedArtworkBugReports { get; set; }

    // Monetization section.
    public IEnumerable<UserDonationTransaction> UserDonationTransactions { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int NickNameLength = 32;

        public const int AvatarUrlLength = 2000;

        public const int AboutMeLength = 2000;
    }
    #endregion

    #region Static Methods
    public static UserProfile NewProfile(long userId, string nickname, string avatarUrl)
    {
        return new UserProfile
        {
            UserId = userId,
            Gender = UserGender.NotSelected,
            NickName = nickname,
            AboutMe = "None",
            AvatarUrl = avatarUrl,
        };
    }
    #endregion
}
