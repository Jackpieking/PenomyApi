using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;
using System.Collections.Generic;

namespace PenomyAPI.App.FeatArt10;

public sealed class Art10Request : IFeatureRequest<Art10Response>
{
    public long ComicId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public AppFileInfo ThumbnailFileInfo { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public IEnumerable<AppFileInfo> ChapterImageFileInfos { get; set; }

    public long CreatedBy { get; set; }

    public DateTime PublishedAt { get; set; }
}
