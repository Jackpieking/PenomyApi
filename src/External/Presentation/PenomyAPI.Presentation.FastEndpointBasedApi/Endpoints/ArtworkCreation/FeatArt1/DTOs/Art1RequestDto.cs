using PenomyAPI.App.FeatArt1;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt1.DTOs;

public sealed class Art1RequestDto
{
    public ArtworkType ArtworkType { get; set; }

    public int PageNumber { get; set; }

    public Art1Request MapToRequest(long creatorId, int pageSize)
    {
        return new Art1Request
        {
            ArtworkType = ArtworkType,
            CreatorId = creatorId,
            PageNumber = PageNumber,
            PageSize = pageSize,
        };
    }
}
