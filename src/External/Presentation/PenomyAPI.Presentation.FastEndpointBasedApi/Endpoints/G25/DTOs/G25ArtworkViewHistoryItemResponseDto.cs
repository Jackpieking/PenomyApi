using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.DTOs;

public class G25ArtworkViewHistoryItemResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public double AverageStarRates { get; set; }

    public string OriginImageUrl { get; set; }

    public string CreatorId { get; set; }

    public string CreatorName { get; set; }

    public string CreatorAvatarUrl { get; set; }

    public G25ChapterViewHistoryItemResponseDto Chapter { get; set; }

    public static G25ArtworkViewHistoryItemResponseDto MapFrom(G25ViewHistoryArtworkReadModel model)
    {
        return new()
        {
            Id = model.Id.ToString(),
            Title = model.Title,
            ThumbnailUrl = model.ThumbnailUrl,
            OriginImageUrl = model.OriginImageUrl,
            ArtworkStatus = model.ArtworkStatus,
            AverageStarRates = model.AverageStarRates,
            // Creator section.
            CreatorId = model.CreatorId.ToString(),
            CreatorName = model.CreatorName,
            CreatorAvatarUrl = model.CreatorAvatarUrl,
            Chapter = new()
            {
                Id = model.Chapter.Id.ToString(),
                UploadOrder = model.Chapter.UploadOrder,
                ViewedAt = model.Chapter.ViewedAt,
            }
        };
    }
}
