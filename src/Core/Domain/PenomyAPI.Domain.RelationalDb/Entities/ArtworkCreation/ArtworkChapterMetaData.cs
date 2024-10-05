using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkChapterMetaData : IEntity
{
    public long ChapterId { get; set; }

    public long TotalViews { get; set; }

    public long TotalFavorites { get; set; }

    public long TotalComments { get; set; }

    public bool HasAdRevenueEnabled { get; set; }

    #region Navigation
    public ArtworkChapter ArtworkChapter { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
    }
    #endregion
}
