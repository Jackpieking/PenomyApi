using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class UserPostAttachedMedia : EntityWithId<long>
{
    public long PostId { get; set; }

    public string FileName { get; set; }

    public UserPostAttachedMediaType MediaType { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public UserPost UserPost { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 2000;
    }
    #endregion
}
