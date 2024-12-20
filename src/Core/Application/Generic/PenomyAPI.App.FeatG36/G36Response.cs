using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatG36;

public sealed class G36Response : IFeatureResponse
{
    public G36ResponseAppCode AppCode { get; set; }

    public string NewAvatarUrl { get; set; }

    public static G36Response SUCCESS(string newAvatarUrl) => new()
    {
        AppCode = G36ResponseAppCode.SUCCESS,
        NewAvatarUrl = newAvatarUrl
    };

    public static readonly G36Response NICKNAME_IS_ALREADY_EXISTED = new()
    {
        AppCode = G36ResponseAppCode.NICKNAME_IS_ALREADY_EXISTED,
    };

    public static readonly G36Response INVALID_FILE_UPLOAD = new()
    {
        AppCode = G36ResponseAppCode.INVALID_FILE_UPLOAD,
    };

    public static readonly G36Response FILE_SERVICE_ERROR = new()
    {
        AppCode = G36ResponseAppCode.FILE_SERVICE_ERROR,
    };

    public static readonly G36Response DATABASE_ERROR = new()
    {
        AppCode = G36ResponseAppCode.DATABASE_ERROR,
    };
}
