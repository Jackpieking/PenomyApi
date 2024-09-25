namespace PenomyAPI.App.Common.Models.ArtworkCreation
{
    public sealed class ImageFileInfo
    {
        /// <summary>
        ///     FileId using SnowflakeId.
        /// </summary>
        public long FileId { get; set; }

        public string FileName { get; set; }

        public int UploadOrder { get; set; }

        public string StorageUrl { get; set; }
    }
}
