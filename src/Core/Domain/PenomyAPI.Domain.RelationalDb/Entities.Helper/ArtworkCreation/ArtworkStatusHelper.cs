using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Domain.RelationalDb.Entities.Helper.ArtworkCreation;

public static class ArtworkStatusHelper
{
    public static string GetLabel(ArtworkStatus status)
    {
        return status switch
        {
            ArtworkStatus.OnGoing => ArtworkStatus.OnGoing.ToString(),
            ArtworkStatus.Finished => ArtworkStatus.Finished.ToString(),
            ArtworkStatus.Cancelled => ArtworkStatus.Cancelled.ToString(),
            _ => ArtworkStatus.OnGoing.ToString()
        };
    }

    public static string GetVietnameseLabel(ArtworkStatus status)
    {
        return status switch
        {
            ArtworkStatus.OnGoing => "Còn cập nhật",
            ArtworkStatus.Finished => "Hoàn thành",
            ArtworkStatus.Cancelled => "Dừng cập nhật",
            _ => "Còn cập nhật"
        };
    }
}
