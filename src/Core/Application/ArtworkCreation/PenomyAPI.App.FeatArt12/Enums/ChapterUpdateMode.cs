namespace PenomyAPI.App.FeatArt12.Enums;

public enum ChapterUpdateMode
{
    /// <summary>
    ///     Indicate to update chapter in drafted mode.
    ///     Which mean the chapter still keeping as a draft but not publish.
    /// </summary>
    Drafted = 1,

    /// <summary>
    ///     Indicate to update chapter in scheduled mode.
    ///     Which mean the chapter is now scheduled to publish at specific time.
    /// </summary>
    Scheduled = 2,

    /// <summary>
    ///     Indicate to update chater in scheduled mode.
    ///     Which mean the chapter is now published immediately.
    /// </summary>
    Published = 3,

    /// <summary>
    ///     Indicate to update the chapter contents only
    ///     without changing its <see cref="Domain.RelationalDb.Entities.ArtworkCreation.ArtworkChapter.PublishStatus" />.
    /// </summary>
    UpdateContentOnly = 4,
}
