using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.DTOs;

public class G28ArtworkResponseDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string ThumbnailUrl { get; set; }

    public ArtworkStatus ArtworkStatus { get; set; }

    public double AverageStarRates { get; set; }

    public string OriginImageUrl { get; set; }

    public G28ChapterResponseDto Chapter { get; set; }

    public static G28ArtworkResponseDto MapFrom(G28ArtworkDetailReadModel model)
    {
        var response = new G28ArtworkResponseDto()
        {
            Id = model.Id.ToString(),
            Title = model.Title,
            ThumbnailUrl = model.ThumbnailUrl,
            OriginImageUrl = model.OriginImageUrl,
            ArtworkStatus = model.ArtworkStatus,
            AverageStarRates = model.AverageStarRates,
        };

        if (!Equals(model.LatestChapter, null))
        {
            response.Chapter = new()
            {
                Id = model.LatestChapter.Id.ToString(),
                UploadOrder = model.LatestChapter.UploadOrder,
                PublishedAt = model.LatestChapter.PublishedAt,
            };
        }

        return response;
    }
}
