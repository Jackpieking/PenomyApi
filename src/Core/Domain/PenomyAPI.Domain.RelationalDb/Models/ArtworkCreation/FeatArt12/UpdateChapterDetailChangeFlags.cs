namespace PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt12;

/// <summary>
///     This class contains related flag support
///     to update the chapter detail properly.
/// </summary>
public sealed class UpdateChapterDetailChangeFlags
{
    /// <summary>
    ///     Flag to indicate this chapter is changed from
    ///     drafted publish status to other publish status.
    /// </summary>
    public bool IsChangedFromDraftedToOtherPublishStatus { get; set; }

    /// <summary>
    ///     Flag to indicate this chapter has changed its
    ///     publish detail (from schedule to published or update the schedule time).
    /// </summary>
    public bool HasChangedInPublishDetail { get; set; }
}
