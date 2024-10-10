namespace PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;

public static class ArtworkConstraints
{
    public static readonly string[] VALID_FILE_EXTENSIONS;

    /// <summary>
    ///     The maximum image file size that can be uploaded
    ///     to the comic thumbnail and comic chapter. 
    /// </summary>
    public const int MAXIMUM_IMAGE_FILE_SIZE = 4 * 1024 * 1024;

    // Init the valid file extensions array when this class is first visited.
    static ArtworkConstraints()
    {
        VALID_FILE_EXTENSIONS = ["jpg", "png", "jpeg"];
    }
}
