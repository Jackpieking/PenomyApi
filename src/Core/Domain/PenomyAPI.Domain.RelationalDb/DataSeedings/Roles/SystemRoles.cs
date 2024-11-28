using PenomyAPI.Domain.RelationalDb.Entities.SystemManagement;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.DataSeedings.Roles;

public static class SystemRoles
{
    private const int TOTAL_SYSTEM_ROLES = 4;

    public static class UserManager
    {
        public const long Id = 12092517130489856;

        public const string RoleName = "user_manager";

        public const string Code = "USER_MANAGER";
    }

    public static class ArtworkManager
    {
        public const long Id = 12077465992220672;

        public const string RoleName = "artwork_manager";

        public const string Code = "ARTWORK_MANAGER";
    }

    public static class SocialMediaManager
    {
        public const long Id = 9070500101804032;

        public const string RoleName = "social_media_manager";

        public const string Code = "SOCIAL_MEDIA_MANAGER";
    }

    public static class SuperManager
    {
        public const long Id = 13616039791218688;

        public const string RoleName = "super_manager";

        public const string Code = "SUPER_MANAGER";
    }

    public static List<SystemRole> GetValues()
    {
        var values = new List<SystemRole>(TOTAL_SYSTEM_ROLES)
        {
            new()
            {
                Id = UserManager.Id,
                Name = UserManager.RoleName,
                Code = UserManager.Code,
                Description = $"{UserManager.RoleName}_{UserManager.Id}"
            },
            new()
            {
                Id = ArtworkManager.Id,
                Name = ArtworkManager.RoleName,
                Code = ArtworkManager.Code,
                Description = $"{ArtworkManager.RoleName}_{ArtworkManager.Id}"
            },
            new()
            {
                Id = SocialMediaManager.Id,
                Name = SocialMediaManager.RoleName,
                Code = SocialMediaManager.Code,
                Description = $"{SocialMediaManager.RoleName}_{SocialMediaManager.Id}"
            },
            new()
            {
                Id = SuperManager.Id,
                Name = SuperManager.RoleName,
                Code = SuperManager.Code,
                Description = $"{SuperManager.RoleName}_{SuperManager.Id}"
            }
        };

        return values;
    }
}
