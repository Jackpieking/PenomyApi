using System;
using System.Collections.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class Series
    : EntityWithId<long>,
        ICreatedEntity<long>,
        IUpdatedEntity<long>,
        ITemporarilyRemovedEntity<long>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public int LastItemOrder { get; set; }

    public string ThumbnailUrl { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long TemporarilyRemovedBy { get; set; }

    public DateTime TemporarilyRemovedAt { get; set; }

    public bool IsTemporarilyRemoved { get; set; }

    #region Navigation
    public UserProfile Creator { get; set; }

    public UserProfile Updater { get; set; }

    public UserProfile Remover { get; set; }

    public IEnumerable<ArtworkSeries> ArtworkSeries { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int TitleLength = 200;

        public const int DescriptionLength = 500;

        public const int ThumbnailUrlLength = 256;
    }
    #endregion
}
