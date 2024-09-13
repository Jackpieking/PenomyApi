namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public enum ArtworkBugSeverity
{
    /// <summary>
    ///     Bug with this severity is affected the user experience
    ///     but do not prevent user to use the features.
    /// </summary>
    Low = 1,

    /// <summary>
    ///     Bug with this severity is affected user experience
    ///     and sometime prevent user to use the features.
    /// </summary>
    Medium = 2,

    /// <summary>
    ///     Bug with this severity will prevent user to use the features
    ///     and provide bad experience.
    /// </summary>
    High = 3
}
