using PenomyAPI.App.FeatArt12.OtherHandlers.GetChapterDetail;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs.GetChapterDetail;

public sealed class Art12GetChapterDetailRequestDto
{
    public long ChapterId { get; set; }

    public Art12GetChapterDetailRequest MapTo(long creatorId)
    {
        return new Art12GetChapterDetailRequest
        {
            ChapterId = ChapterId,
            CreatorId = creatorId
        };
    }
}
