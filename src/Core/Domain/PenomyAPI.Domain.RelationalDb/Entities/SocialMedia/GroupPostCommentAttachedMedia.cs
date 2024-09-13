using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.SocialMedia;

public sealed class GroupPostCommentAttachedMedia : EntityWithId<long>
{
    public long CommentId { get; set; }

    public string FileName { get; set; }

    public GroupPostCommentAttachedMediaType MediaType { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public GroupPostComment Comment { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 256;
    }
    #endregion
}
