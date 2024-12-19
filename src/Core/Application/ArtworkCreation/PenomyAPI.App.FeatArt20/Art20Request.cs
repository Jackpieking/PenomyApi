using PenomyAPI.App.Common;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;

namespace PenomyAPI.App.FeatArt20;

public class Art20Request : IFeatureRequest<Art20Response>
{
    public long AnimeId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public ImageFileInfo ThumbnailFileInfo { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public DateTime PublishedAt { get; set; }

    public VideoFileInfo ChapterVideoFileInfo { get; set; }

    public long CreatedBy { get; set; }

    public bool HasThumbnailFile() => ThumbnailFileInfo != null;
}
