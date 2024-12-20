using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;

public sealed class G35GetCreatorProfileRequest
    : IFeatureRequest<G35GetCreatorProfileResponse>
{
    public long CreatorId { get; set; }
}
