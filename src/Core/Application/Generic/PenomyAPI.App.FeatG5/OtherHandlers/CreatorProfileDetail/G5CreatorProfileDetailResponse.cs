using PenomyAPI.App.Common;
using PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG5;

namespace PenomyAPI.App.FeatG5.OtherHandlers.CreatorProfileDetail;

public class G5CreatorProfileDetailResponse : IFeatureResponse
{
    public G5CreatorProfileResponseAppCode AppCode { get; set; }

    public G5CreatorProfileReadModel CreatorProfile { get; set; }

    public static readonly G5CreatorProfileDetailResponse CREATOR_NOT_FOUND = new()
    {
        AppCode = G5CreatorProfileResponseAppCode.CREATOR_NOT_FOUND,
    };
}
