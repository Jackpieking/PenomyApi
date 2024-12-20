using PenomyAPI.App.FeatArt22.OtherHandlers.GetChapterDetail;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt22.DTOs;

public class Art22GetChapterDetailRequestDto
{
    public long ChapterId { get; set; }

    public Art22GetChapterDetailRequest MapTo(long creatorId)
    {
        return new Art22GetChapterDetailRequest
        {
            ChapterId = ChapterId,
            CreatorId = creatorId
        };
    }
}
