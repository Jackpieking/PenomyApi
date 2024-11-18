using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using System.Collections.Generic;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG2;

/// <summary>
///     The view model that used to get the detail of top
///     recommended artwork from the DB.
/// </summary>
public sealed class G2TopArtworkReadModel
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Introduction { get; set; }

    public string ThumbnailUrl { get; set; }

    public string OriginImageUrl { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public ArtworkMetaData ArtworkMetaData { get; set; }

    public IEnumerable<Category> ArtworkCategories { get; set; }

    public UserProfile Creator { get; set; }

    /// <summary>
    ///     The upload order of the latest chapter
    ///     that belonged to this artwork
    /// </summary>
    public int LastChapterUploadOrder { get; set; }

    /// <summary>
    ///     This property is supported for artwork type animation.
    /// </summary>
    public int FixedTotalChapters { get; set; }

    public long LatestChapterId { get; set; }

    public string LatestChapterTitle { get; set; }
}
