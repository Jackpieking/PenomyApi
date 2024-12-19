using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt12;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Domain.RelationalDb.Entities.Contraints.ArtworkCreation;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text.Json;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt12.DTOs;

public sealed class Art12RequestDto
{
    public long ComicId { get; set; }

    public long ChapterId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    /// <summary>
    ///     The image file that will be used to update the thumbnail for current chapter.
    /// </summary>
    public IFormFile ThumbnailImageFile { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public App.FeatArt12.Enums.ChapterUpdateMode UpdateMode { get; set; }

    public DateTime ScheduledAt { get; set; }

    /// <summary>
    ///     The json string contains the detail of chapter media items
    ///     that will be later deserialzed to process.
    /// </summary>
    public string ChapterMediaItemsInJson { get; set; }

    private IDictionary<string, ChapterMediaUpdatedInfoItem> _chapterMediaUpdatedInfoItems { get; set; }

    /// <summary>
    ///     The list of chapter image files that are uploaded new.
    /// </summary>
    public IFormFileCollection NewChapterImageFiles { get; set; }

    public bool HasNewUploadThumbnailFile()
    {
        return !Equals(ThumbnailImageFile, null);
    }

    public bool HasNewUploadImageFiles()
    {
        return !Equals(NewChapterImageFiles, null);
    }

    /// <summary>
    ///     Deserialize the ChapterMediaItemsInJson to get the detail of updating.
    /// </summary>
    /// <returns>
    ///     The result after deserializing.
    /// </returns>
    public Result<IDictionary<string, ChapterMediaUpdatedInfoItem>> DeserializeChapterMediaUpdatedInfoItems()
    {
        // Check if the input chapter medias in json after trim.
        // IsNullOrEmpty or equal empty_array_in_json before go to deserialize process.
        ChapterMediaItemsInJson = $"{ChapterMediaItemsInJson}".Trim();
        const string EMPTY_ARRAY_IN_JSON = "[]";

        if (String.IsNullOrEmpty(ChapterMediaItemsInJson)
            || EMPTY_ARRAY_IN_JSON.Equals(ChapterMediaItemsInJson))
        {
            return Result<IDictionary<string, ChapterMediaUpdatedInfoItem>>.Failed();
        }

        try
        {
            var chapterMediaUpdatedInfoItems = JsonSerializer.Deserialize<IEnumerable<ChapterMediaUpdatedInfoItem>>(
                ChapterMediaItemsInJson,
                JsonSerializerOptionsProvider.Get());

            _chapterMediaUpdatedInfoItems = chapterMediaUpdatedInfoItems.ToFrozenDictionary(
                updatedInfoItems => updatedInfoItems.Id);

            return Result<IDictionary<string, ChapterMediaUpdatedInfoItem>>.Success(
                _chapterMediaUpdatedInfoItems);
        }
        catch
        {
            return Result<IDictionary<string, ChapterMediaUpdatedInfoItem>>.Failed();
        }
    }

    public IDictionary<string, ChapterMediaUpdatedInfoItem> GetChapterMediaUpdatedInfoItems()
    {
        return _chapterMediaUpdatedInfoItems;
    }

    /// <summary>
    ///     Verify all the upload image files to ensure the validity.
    /// </summary>
    /// <returns>
    ///     Return <see langword="true"/> if all upload image files are valid.
    /// </returns>
    public bool ValidateAllUploadImageFiles()
    {
        var formFileHelper = FormFileHelper.Instance;

        // Verify the thumbnail image file first, if have.
        if (HasNewUploadThumbnailFile())
        {
            if (!formFileHelper.IsValidImageFile(ThumbnailImageFile))
            {
                return false;
            }
        }

        // Verify all uploaded chapter image files, if have.
        if (HasNewUploadImageFiles())
        {
            // If found any find has invalid format or
            // not conform the constraint, then return false.
            foreach (var file in NewChapterImageFiles)
            {
                if (file.Length > ArtworkConstraints.MAXIMUM_IMAGE_FILE_SIZE
                    || !formFileHelper.IsValidImageFile(file))
                {
                    return false;
                }
            }
        }

        return true;
    }

    #region Map related form files into media updated info items.
    private bool _completedMapRelatedFormFiles = false;

    public void MapRelatedFormFilesIntoMediaUpdatedInfoItems()
    {
        // Check if the map operation is already completed or not.
        // If completed then return.
        if (_completedMapRelatedFormFiles)
        {
            return;
        }

        var formFileHelper = FormFileHelper.Instance;

        foreach (var imageFile in NewChapterImageFiles)
        {
            var fileId = formFileHelper.GetFileNameWithoutExtension(imageFile);

            _chapterMediaUpdatedInfoItems.TryGetValue(fileId, out var chapterMediaUpdatedInfoItem);

            if (chapterMediaUpdatedInfoItem != null)
            {
                chapterMediaUpdatedInfoItem.SetRelatedFormFile(imageFile);
            }
        }

        // Mark the operation as completed.
        _completedMapRelatedFormFiles = true;
    }
    #endregion

    public Result<Art12Request> TryMapToRequest(long creatorId)
    {
        var request = new Art12Request
        {
            CreatorId = creatorId,
            ComicId = ComicId,
            ChapterId = ChapterId,
            Title = Title,
            Description = Description,
            AllowComment = AllowComment,
            UpdateMode = UpdateMode,
            PublicLevel = PublicLevel,
            ScheduledAt = ScheduledAt.ToUniversalTime(),
        };

        // Set the thumbnail file to the feature request body.
        if (HasNewUploadThumbnailFile())
        {
            request.ThumbnailFileInfo = new ImageFileInfo
            {
                FileDataStream = ThumbnailImageFile.OpenReadStream(),
                FileSize = ThumbnailImageFile.Length,
                FileName = ThumbnailImageFile.FileName,
            };
        }

        // Traverse through the updateInfoItems and add the items to their correct list to process. 
        var chapterMediaUpdatedInfoItems = _chapterMediaUpdatedInfoItems.Values;

        // This dictionary will be used to check if any upload order is duplicated
        // or not to prevent user input invalid chapter media item.
        var uploadOrderDictionary = new Dictionary<int, int>();

        // Variables to verify if the input chapter media id is valid to parse as long or not.
        bool canParse = true;
        long chapterMediaId = default;

        foreach (var infoItem in chapterMediaUpdatedInfoItems)
        {
            // Resolve the item that has change in upload order and 
            if (infoItem.IsMarkedAsChangedInUploadOrder())
            {
                // Init the list when first item has change in upload order.
                request.InitUpdatedChapterMediaList();

                // Check if the id of current info item is valid to parse or not.
                canParse = long.TryParse(infoItem.Id, out chapterMediaId);

                if (!canParse)
                {
                    return Result<Art12Request>.Failed();
                }

                var updatedChapterMediaItem = new ArtworkChapterMedia
                {
                    Id = chapterMediaId,
                    UploadOrder = infoItem.NewUploadOrder,
                };

                // Add the item into update chapter media list.
                request.UpdatedChapterMedias.Add(updatedChapterMediaItem);

                // Try add the upload order to dictionary to check if duplication found.
                var isDuplicated = !uploadOrderDictionary.TryAdd(infoItem.NewUploadOrder, default);

                if (isDuplicated)
                {
                    return Result<Art12Request>.Failed();
                }
            }
            else if (infoItem.IsMarkedAsDeleted())
            {
                // Init the deleted list when first item is marked as deleted.
                request.InitDeletedChapterMediaIdList();

                // Check if the id of current info item is valid to parse or not.
                canParse = long.TryParse(infoItem.Id, out chapterMediaId);

                if (!canParse)
                {
                    return Result<Art12Request>.Failed();
                }

                request.DeletedChapterMediaIds.Add(chapterMediaId);
            }
            else if (infoItem.IsMarkedAsNewUploaded())
            {
                // Init the chapter image file list when first item is marked as new uploaded.
                request.InitNewChapterImageFileInfoList();

                var chapterImageFileInfo = new ImageFileInfo
                {
                    FileId = infoItem.Id,
                    FileName = infoItem.GetRelatedFormFile().FileName,
                    FileDataStream = infoItem.GetRelatedFormFile().OpenReadStream(),
                    FileSize = infoItem.GetRelatedFormFile().Length,
                    UploadOrder = infoItem.NewUploadOrder,
                };

                request.NewChapterImageFileInfos.Add(chapterImageFileInfo);

                // Try add the upload order to dictionary to check if duplication found.
                var isDuplicated = !uploadOrderDictionary.TryAdd(infoItem.NewUploadOrder, default);

                if (isDuplicated)
                {
                    return Result<Art12Request>.Failed();
                }
            }
        }

        return Result<Art12Request>.Success(request);
    }
}
