namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

public enum PublishStatus
{
    Drafted = 1,

    Published = 2,

    Scheduled = 3,

    /// <summary>
    ///     Indicate the publish status
    ///     that only need to update
    /// </summary>
    UpdatedOnly = 4,
}
