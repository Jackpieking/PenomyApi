using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Helper.ArtworkCreation;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;

public class PublicLevelDto
{
    public int Id { get; set; }

    public string Label { get; set; }

    public static PublicLevelDto CovertToDto(ArtworkPublicLevel publicLevel)
    {
        return new PublicLevelDto
        {
            Id = (int)publicLevel,
            Label = ArtworkPublicLevelHelper.GetLabel(publicLevel)
        };
    }
}
