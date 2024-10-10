using System.Collections.ObjectModel;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25ResponseDto
{
    public long ArtworkId { get; set; }
    public ArtworkType artworkType { get; set; }
    public string ArtworkTitle { get; set; }
    public long AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string ThumbnailUrl { get; set; }
    public long TotalFavorites { get; set; }
    public long TotalStarRates { get; set; }
    public Collection<G25ChapterDto> G25Chapters { get; set; }
}
