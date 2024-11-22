using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG37;

public sealed class G37Response : IFeatureResponse
{
    public G37ResponseAppCode AppCode { get; set; }

    public static G37Response SUCCESS = new()
    {
        AppCode = G37ResponseAppCode.SUCCESS,
    };

    public static G37Response USER_HAS_ALREADY_BECOME_CREATOR = new()
    {
        AppCode = G37ResponseAppCode.USER_HAS_ALREADY_BECOME_CREATOR,
    };

    public static G37Response DATABASE_ERROR = new()
    {
        AppCode = G37ResponseAppCode.DATABASE_ERROR,
    };
}
