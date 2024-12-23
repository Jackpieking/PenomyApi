using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class BugReportAttachedMedia : EntityWithId<long>
{
    public long BugReportId { get; set; }

    public string FileName { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public BugReport BugReport { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 2000;

        public const int StorageUrlLength = 2000;
    }
    #endregion
}
