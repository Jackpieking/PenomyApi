namespace PenomyAPI.App.FeatArt16;

public enum Art16ResponseAppCode
{
    SUCCESS = 0,

    ARTWORK_ID_NOT_FOUND,

    /// <summary>
    ///     Specify if other creator try to access the detail
    ///     of the comic that not belonged to their authorized artworks.
    /// </summary>
    ARTWORK_IS_NOT_AUTHORIZED_TO_CURRENT_CREATOR,

    ARTWORK_IS_TEMPORARILY_REMOVED,

    DATABASE_ERROR,
}
