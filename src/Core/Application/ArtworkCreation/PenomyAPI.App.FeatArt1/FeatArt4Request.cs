using PenomyAPI.App.Common;
using PenomyAPI.App.Common.Models.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.App.FeatArt4;

public class FeatArt4Request : IFeatureRequest<FeatArt4Response>
{
    public long ComicId { get; set; }

    public string Title { get; set; }

    public ImageFileInfo ThumbnailFileInfo { get; set; }

    public long OriginId { get; set; }

    public string Introduction { get; set; }

    public string AuthorName { get; set; }

    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public long CreatedBy { get; set; }
}
