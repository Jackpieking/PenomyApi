using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs;

/// <summary>
///     This class contain the updated detail of the specified
///     chapter media item, including the old and new upload order of that item.
/// </summary>
public sealed class ChapterMediaUpdatedInfoItem
{
    private const int DELETED_UPLOAD_ORDER = -1;

    /// <summary>
    ///     This IFormFile is only used when the chapter media item
    ///     has uploaded new image file to update the state.
    /// </summary>
    private IFormFile _relatedFormFile;

    public string Id { get; set; }

    public int OldUploadOrder { get; set; }

    public int NewUploadOrder { get; set; }

    public void SetRelatedFormFile(IFormFile relatedFormFile)
    {
        _relatedFormFile = relatedFormFile;
    }

    public IFormFile GetRelatedFormFile()
    {
        return _relatedFormFile;
    }

    public bool HasRelatedFormFile()
    {
        return _relatedFormFile != null;
    }

    /// <summary>
    ///     Check if the current chapter media item has any change
    ///     from its upload order, not marked as deleted and has no related form file.
    /// </summary>
    /// <returns>
    ///     Return <see langword="true"/> if change is found.
    /// </returns>
    public bool IsMarkedAsChangedInUploadOrder()
    {
        // To be marked as changed in upload order only.
        // the media item has different in old and new upload order
        // and not marked as deleted nor contain related form file.
        return OldUploadOrder != NewUploadOrder
            && NewUploadOrder != DELETED_UPLOAD_ORDER
            && Equals(_relatedFormFile, null);
    }

    /// <summary>
    ///     Check if the current chapter media item is marked as deleted or not.
    /// </summary>
    /// <returns>
    ///     Return <see langword="true"/> if it is marked as deleted.
    /// </returns>
    public bool IsMarkedAsDeleted()
    {
        return NewUploadOrder == DELETED_UPLOAD_ORDER;
    }

    /// <summary>
    ///     Check if the current chapter media item has its related form file or not.
    /// </summary>
    /// <returns></returns>
    public bool IsMarkedAsNewUploaded()
    {
        return !Equals(_relatedFormFile, null);
    }
}
