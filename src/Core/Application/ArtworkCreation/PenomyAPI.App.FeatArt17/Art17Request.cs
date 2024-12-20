using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt17;

public class Art17Request : IFeatureRequest<Art17Response>
{
    public long AnimeId { get; set; }

    public string Title { get; set; }

    public AppFileInfo ThumbnailFileInfo { get; set; }

    /// <summary>
    ///     Indicate this property to true if the thumbnail
    ///     of the comic is updated by the creator.
    /// </summary>
    public bool IsThumbnailUpdated { get; set; }

    public long OriginId { get; set; }

    public string Introduction { get; set; }

    public string AuthorName { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    /// <summary>
    ///     Indicate this property to true if the category
    ///     list of this comic is updated by the creator.
    /// </summary>
    public bool IsCategoriesUpdated { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public long UpdatedBy { get; set; }
}
