using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadComicDetail;

public sealed class Art7LoadComicDetailResponse : IFeatureResponse
{
    public long ComicId { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Title { get; set; }

    public long OriginId { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public string Introduction { get; set; }

    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }
}
