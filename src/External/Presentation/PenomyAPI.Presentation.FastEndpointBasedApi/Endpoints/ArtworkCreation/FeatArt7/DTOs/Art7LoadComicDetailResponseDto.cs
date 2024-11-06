using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;

public class Art7LoadComicDetailResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string OriginId { get; set; }

    public string Introduction { get; set; }

    public string ThumbnailUrl { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public IEnumerable<Art7ArtworkCategoryDto> SelectedCategories { get; set; }

    public bool AllowComment { get; set; }
}
