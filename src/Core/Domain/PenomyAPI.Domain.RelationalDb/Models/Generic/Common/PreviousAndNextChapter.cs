namespace PenomyAPI.Domain.RelationalDb.Models.Generic.Common;

public sealed record PreviousAndNextChapter
{
    /// <summary>
    ///     Id of the previous chapter of the specified chapter.
    /// </summary>
    public long PrevChapterId { get; set; }

    /// <summary>
    ///     Id of the next chapter of the specified chapter.
    /// </summary>
    public long NextChapterId { get; set; }
}
