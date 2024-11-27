using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;

public static class UserRoles
{
    private const int TOTAL_USER_ROLES = 5;

    public static class Creator
    {
        public const long Id = 9070500101804032;

        public const string RoleName = "creator";

        public const string NormalizedRoleName = "CREATOR";
    }

    public static class GroupManager
    {
        public const long Id = 19073011663556608;

        public const string RoleName = "group_manager";

        public const string NormalizedRoleName = "GROUP_MANAGER";
    }

    public static class GroupMember
    {
        public const long Id = 20825912371105792;

        public const string RoleName = "group_member";

        public const string NormalizedRoleName = "GROUP_MEMBER";
    }

    public static class ChatGroupManager
    {
        public const long Id = 12092744503709696;

        public const string RoleName = "chat_group_manager";

        public const string NormalizedRoleName = "CHAT_GROUP_MANAGER";
    }

    public static class ChatGroupMember
    {
        public const long Id = 12092517130489856;

        public const string RoleName = "chat_group_member";

        public const string NormalizedRoleName = "CHAT_GROUP_MEMBER";
    }

    public static List<Role> GetValues()
    {
        var values = new List<Role>(TOTAL_USER_ROLES)
        {
            new()
            {
                Id = Creator.Id,
                Name = Creator.RoleName,
                NormalizedName = Creator.NormalizedRoleName,
                ConcurrencyStamp = $"{Creator.RoleName}_{Creator.Id}"
            },
            new()
            {
                Id = GroupManager.Id,
                Name = GroupManager.RoleName,
                NormalizedName = GroupManager.NormalizedRoleName,
                ConcurrencyStamp = $"{GroupManager.RoleName}_{GroupManager.Id}"
            },
            new()
            {
                Id = GroupMember.Id,
                Name = GroupMember.RoleName,
                NormalizedName = GroupMember.NormalizedRoleName,
                ConcurrencyStamp = $"{GroupMember.RoleName}_{GroupMember.Id}"
            },
            new()
            {
                Id = ChatGroupManager.Id,
                Name = ChatGroupManager.RoleName,
                NormalizedName = ChatGroupManager.NormalizedRoleName,
                ConcurrencyStamp = $"{ChatGroupManager.RoleName}_{ChatGroupManager.Id}"
            },
            new()
            {
                Id = ChatGroupMember.Id,
                Name = ChatGroupMember.RoleName,
                NormalizedName = ChatGroupMember.NormalizedRoleName,
                ConcurrencyStamp = $"{ChatGroupMember.RoleName}_{ChatGroupMember.Id}"
            }
        };

        return values;
    }
}
