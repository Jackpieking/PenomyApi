namespace PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;

public static class ArtworkConstraints
{
    public static readonly string[] VALID_IMAGE_FILE_EXTENSIONS;
    public static readonly string[] VALID_VIDEO_FILE_EXTENSIONS;

    /// <summary>
    ///     The maximum image file size that can be uploaded
    ///     to the comic thumbnail and comic chapter. (Current value: 2MB)
    /// </summary>
    public const int MAXIMUM_IMAGE_FILE_SIZE = 2 * 1024 * 1024;

    /// <summary>
    ///     The maximum video file size that can be uploaded
    ///     to the anime video chapter. (Current value: 500MB)
    /// </summary>
    public const long MAXIMUM_VIDEO_FILE_SIZE = 500 * 1024 * 1024;

    /// <summary>
    ///     The maximum total image file size that can be uploaded
    ///     to the comic thumbnail and comic chapter. (Current value: 32MB)
    /// </summary>
    public const int MAXIMUM_TOTAL_CHAPTER_IMAGE_FILE_SIZE = 32 * 1024 * 1024;

    // Init the valid file extensions array when this class is first visited.
    static ArtworkConstraints()
    {
        VALID_IMAGE_FILE_EXTENSIONS = ["jpg", "png", "jpeg"];
        VALID_VIDEO_FILE_EXTENSIONS = ["mp4", "mkv", "x-matroska"];
    }
}
