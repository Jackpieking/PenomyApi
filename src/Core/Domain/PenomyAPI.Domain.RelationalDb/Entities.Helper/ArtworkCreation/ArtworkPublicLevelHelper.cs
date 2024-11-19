using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.Domain.RelationalDb.Entities.Helper.ArtworkCreation;

public static class ArtworkPublicLevelHelper
{
    public static string GetLabel(ArtworkPublicLevel publicLevel)
    {
        return publicLevel switch
        {
            ArtworkPublicLevel.Everyone => "Công khai (ai cũng có thể xem)",
            ArtworkPublicLevel.OnlyFriend => "Bạn bè (Bạn và bạn bè của bạn)",
            ArtworkPublicLevel.Private => "Riêng tư (chỉ riêng bạn)",
            ArtworkPublicLevel.PrivateWithLimitedUsers => "Giới hạn số người được xem",
            _ => "Công khai (ai cũng có thể xem)"
        };
    }
}
