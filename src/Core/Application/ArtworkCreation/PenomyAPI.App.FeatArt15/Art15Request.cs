using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt15;

public class Art15Request : IFeatureRequest<Art15Response>
{
    public long ArtworkId { get; set; }

    public string Title { get; set; }

    public AppFileInfo ThumbnailFileInfo { get; set; }

    public long OriginId { get; set; }

    public string Introduction { get; set; }

    public string AuthorName { get; set; }

    public IEnumerable<ArtworkCategory> ArtworkCategories { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public long CreatedBy { get; set; }
}
