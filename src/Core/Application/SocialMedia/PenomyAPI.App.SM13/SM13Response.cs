using PenomyAPI.App.Common;

namespace PenomyAPI.App.SM13;

public class SM13Response : IFeatureResponse
{
    public static readonly SM13Response UserPostNotFound =
        new() { IsSuccess = false, StatusCode = SM13ResponseStatusCode.USER_POST_NOT_FOUND };

    public static readonly SM13Response DatabaseError =
        new() { IsSuccess = false, StatusCode = SM13ResponseStatusCode.DATABASE_ERROR };

    public static readonly SM13Response FileServiceError =
        new() { IsSuccess = false, StatusCode = SM13ResponseStatusCode.FILE_SERVICE_ERROR };

    public bool IsSuccess { get; set; }

    public SM13ResponseStatusCode StatusCode { get; set; }
}
