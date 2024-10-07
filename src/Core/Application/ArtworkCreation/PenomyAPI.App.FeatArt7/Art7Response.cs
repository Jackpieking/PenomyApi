using PenomyAPI.App.Common;

namespace PenomyAPI.App.FeatArt7;

public sealed class Art7Response : IFeatureResponse
{
    public bool IsSuccess { get; set; }

    public Art7ResponseStatusCode StatusCode { get; set; }

    #region Static readonly instances
    public static readonly Art7Response ComicIdNotFound = new()
    {
        IsSuccess = false,
        StatusCode = Art7ResponseStatusCode.COMIC_ID_NOT_FOUND
    };

    public static readonly Art7Response DatabaseError = new()
    {
        IsSuccess = false,
        StatusCode = Art7ResponseStatusCode.DATABASE_ERROR
    };

    public static readonly Art7Response FileServiceError = new()
    {
        IsSuccess = false,
        StatusCode = Art7ResponseStatusCode.FILE_SERVICE_ERROR
    };
    #endregion
}
