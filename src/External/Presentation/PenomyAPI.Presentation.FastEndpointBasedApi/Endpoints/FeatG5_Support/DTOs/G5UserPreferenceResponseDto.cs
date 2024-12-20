using ProtoBuf;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG5_Support.DTOs;

public class G5UserPreferenceResponseDto
{
    public string FirstChapterId { get; set; }

    public string LastReadChapterId { get; set; }

    public bool IsUserFavorite { get; set; }

    public bool HasFollowed { get; set; }
}
