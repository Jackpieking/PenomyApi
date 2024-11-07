namespace PenomyAPI.App.FeatArt12.Enums;

public enum ChapterUpdateMode
{
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
    ///     Indicate to update the chapter contents only. The update
    ///     information includes chapter images, title but not affect to its upload order
    ///     or publish status.
    /// </summary>
    UpdateContentOnly = 4,
}
