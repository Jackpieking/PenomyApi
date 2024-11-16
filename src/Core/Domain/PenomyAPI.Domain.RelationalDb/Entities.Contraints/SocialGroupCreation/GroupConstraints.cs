namespace PenomyAPI.Domain.RelationalDb.Entities.Contraints.SocialGroupCreation;

public static class GroupConstraints
{
    public static readonly string[] VALID_FILE_EXTENSIONS;

    /// <summary>
    ///     The maximum image file size that can be uploaded
    ///     to the group cover. (Current value: 4MB)
    /// </summary>
    public const int MAXIMUM_IMAGE_FILE_SIZE = 10 * 1024 * 1024;

    // Init the valid file extensions array when this class is first visited.
    static GroupConstraints()
    {
        VALID_FILE_EXTENSIONS = ["jpg", "png", "jpeg"];
    }
}
