using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM14;

public class SM14Response : IFeatureResponse
{
    public static readonly SM14Response SUCCESS = new()
    {
        AppCode = SM14ResponseStatusCode.SUCCESS
    };

    public static readonly SM14Response NOT_FOUND = new()
    {
        AppCode = SM14ResponseStatusCode.POST_NOT_FOUND
    };


    public static readonly SM14Response DATABASE_ERROR = new()
    {
        AppCode = SM14ResponseStatusCode.DATABASE_ERROR
    };

    public static readonly SM14Response FILE_SERVICE_ERROR = new()
    {
        AppCode = SM14ResponseStatusCode.FILE_SERVICE_ERROR
    };

    public SM14ResponseStatusCode AppCode { get; set; }
}
