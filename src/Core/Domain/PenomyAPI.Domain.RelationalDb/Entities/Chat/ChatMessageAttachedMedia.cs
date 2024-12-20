using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Chat;

public sealed class ChatMessageAttachedMedia : EntityWithId<long>
{
    public long ChatMessageId { get; set; }

    public string FileName { get; set; }

    public ChatMessageAttachedMediaType MediaType { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public ChatMessage ChatMessage { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 2000;

        public const int StorageUrlLength = 2000;
    }
    #endregion
}
