namespace PenomyAPI.App.FeatArt5;

public enum Art5ResponseAppCode
{
    SUCCESS = 0,

    COMIC_ID_NOT_FOUND,

    /// <summary>
    ///     Specify if other creator try to access the detail
    ///     of the comic that not belonged to their authorized artworks.
    /// </summary>
    COMIC_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR,

    COMIC_IS_TEMPORARILY_REMOVED,

    DATABASE_ERROR,
}
