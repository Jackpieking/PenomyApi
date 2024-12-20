using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt5.DTOs;

public sealed class ComicDetailResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public string Introduction { get; set; }

    public string Origin { get; set; }

    public string AuthorName { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public SeriesDto Series { get; set; }

    public IEnumerable<CategoryDto> Categories { get; set; }
}
