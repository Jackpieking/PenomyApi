using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkChapter
    : EntityWithId<long>,
        ICreatedEntity<long>,
        IUpdatedEntity<long>,
        ITemporarilyRemovedEntity<long>
{
    public const int DraftedUploadOrder = -1;

    public long ArtworkId { get; set; }

    public string Title { get; set; }

    public int UploadOrder { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public string ThumbnailUrl { get; set; }

    public ChapterStatus ChapterStatus { get; set; }

    public bool AllowComment { get; set; }

    public long TotalViews { get; set; }

    public long TotalFavorites { get; set; }

    public long TotalComments { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime PublishedAt { get; set; }

    public long TemporarilyRemovedBy { get; set; }

    public DateTime TemporarilyRemovedAt { get; set; }

    public bool IsTemporarilyRemoved { get; set; }

    #region Navigation
    public Artwork BelongedArtwork { get; set; }

    public UserProfile Creator { get; set; }

    public UserProfile Updater { get; set; }

    public UserProfile Remover { get; set; }

    public IEnumerable<ArtworkChapterMedia> ChapterMedias { get; set; }

    public IEnumerable<ArtworkChapterReport> ChapterReports { get; set; }

    public IEnumerable<UserArtworkViewHistory> UserChapterViewHistories { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int ThumbnailUrlLength = 256;
    }
    #endregion
}
