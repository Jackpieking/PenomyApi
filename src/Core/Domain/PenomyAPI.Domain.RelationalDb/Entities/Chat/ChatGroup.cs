using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatGroup : EntityWithId<long>, ICreatedEntity<long>
{
    public ChatGroupType ChatGroupType { get; set; }

    public string GroupName { get; set; }

    public string CoverPhotoUrl { get; set; }

    public bool IsPublic { get; set; }

    public int TotalMembers { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    #region Navigation
    public UserProfile Creator { get; set; }

    public IEnumerable<ChatGroupJoinRequest> ChatGroupJoinRequests { get; set; }

    public IEnumerable<UserChatGroupActiveHistory> ChatGroupActiveHistories { get; set; }

    public IEnumerable<ChatGroupMember> ChatGroupMembers { get; set; }

    public IEnumerable<ChatMessage> ChatMessages { get; set; }

    public SocialGroupLinkedChatGroup SocialGroupLinkedChatGroup { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int GroupNameLength = 200;

        public const int CoverPhotoUrlLength = 256;
    }
    #endregion
}
