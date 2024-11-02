using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt6.DTOs;

public class ComicChapterDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public int UploadOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public PublishStatus PublishStatus { get; set; }

    public string ThumbnailUrl { get; set; }

    public bool AllowComment { get; set; }

    public long TotalComments { get; set; }

    public long TotalViews { get; set; }

    public long TotalFavorites { get; set; }
}
