using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;

namespace PenomyAPI.App.FeatG35;

public sealed class G35Response : IFeatureResponse
{
    public UserProfile UserProfile { get; set; }

    public bool IsProfileOwner { get; set; }

    public G35ResponseAppCode AppCode { get; set; }

    public static G35Response USER_ID_NOT_FOUND = new()
    {
        AppCode = G35ResponseAppCode.USER_ID_NOT_FOUND
    };

    public static G35Response SUCCESS(UserProfile userProfile, bool isProfileOwner)
    {
        return new G35Response
        {
            IsProfileOwner = isProfileOwner,
            UserProfile = userProfile,
            AppCode = G35ResponseAppCode.SUCCESS,
        };
    }
}
