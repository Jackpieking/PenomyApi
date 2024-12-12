using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G48.DTOs;

public class G48FavoriteArtworkResponseDto
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

    public G48ChapterResponseDto Chapter { get; set; }

    public static G48FavoriteArtworkResponseDto MapFrom(G48FavoriteArtworkReadModel model)
    {
        var response = new G48FavoriteArtworkResponseDto()
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
            CreatorAvatarUrl = model.CreatorAvatarUrl
        };

        if (!Equals(response.Chapter, null))
        {
            response.Chapter = new()
            {
                Id = model.Chapter.Id.ToString(),
                UploadOrder = model.Chapter.UploadOrder,
                PublishedAt = model.Chapter.PublishedAt,
            };
        }

        return response;
    }
}
