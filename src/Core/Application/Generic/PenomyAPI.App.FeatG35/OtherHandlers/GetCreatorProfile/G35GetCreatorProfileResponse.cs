using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.FeatG35.OtherHandlers.GetCreatorProfile;

public sealed class G35GetCreatorProfileResponse : IFeatureResponse
{
    public UserProfile CreatorProfile { get; set; }

    public G35GetCreatorProfileResponseAppCode AppCode { get; set; }

    public static G35GetCreatorProfileResponse CREATOR_ID_NOT_FOUND = new()
    {
        AppCode = G35GetCreatorProfileResponseAppCode.CREATOR_ID_NOT_FOUND
    };

    public static G35GetCreatorProfileResponse SUCCESS(UserProfile creatorProfile)
    {
        return new G35GetCreatorProfileResponse
        {
            CreatorProfile = creatorProfile,
            AppCode = G35GetCreatorProfileResponseAppCode.SUCCESS,
        };
    }
}
