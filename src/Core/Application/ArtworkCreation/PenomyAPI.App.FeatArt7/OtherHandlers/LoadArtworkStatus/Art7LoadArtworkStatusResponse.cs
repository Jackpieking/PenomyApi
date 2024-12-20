using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

namespace PenomyAPI.App.FeatArt7.OtherHandlers.LoadArtworkStatus;

public sealed class Art7LoadArtworkStatusResponse : IFeatureResponse
{
    public ArtworkStatus[] ArtworkStatuses { get; set; }
}
