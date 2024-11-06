using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt7;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt7.DTOs;

public sealed class Art7RequestDto
{
    /// <summary>
    ///     A singleton instance of json serializer
    ///     options to reuse for next time.
    /// </summary>
    private static JsonSerializerOptions _jsonSerializerOptions;
    private static object _lock = new();

    [Required]
    public long ComicId { get; set; }

    public string Title { get; set; }

    [Required]
    public IFormFile ThumbnailImageFile { get; set; }

    public long OriginId { get; set; }

    public string Introduction { get; set; }

    /// <summary>
    ///     This property is store the json value with
    ///     schema as an array of <see cref="CategoryDto"/>.
    /// </summary>
    [Required]
    public string SelectedCategories { get; set; }

    public IEnumerable<CategoryDto> ArtworkCategories { get; private set; }

    [Required]
    public bool IsCategoriesUpdated { get; set; }

    public ArtworkPublicLevel PublicLevel { get; set; }

    public bool AllowComment { get; set; }

    public bool ConfirmPolicy { get; set; }

    /// <summary>
    ///     Check if the input selected categories have
    ///     valid json schema or not to parse.
    /// </summary>
    /// <returns>
    ///     (bool) result indicate the valid of the input.
    /// </returns>
    public bool IsValidSelectedCategoriesInput()
    {
        var deserializedResult = DeserializedSelectedCategories();

        if (!deserializedResult.IsSuccess)
        {
            return false;
        }

        ArtworkCategories = deserializedResult.Value;

        // Check if any input category id is invalid format or not.
        foreach (var item in ArtworkCategories)
        {
            if (!long.TryParse(item.Id, out _))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsThumbnailUpdated()
    {
        return ThumbnailImageFile != null;
    }

    private Result<IEnumerable<CategoryDto>> DeserializedSelectedCategories()
    {
        try
        {
            var selectedCategories = JsonSerializer.Deserialize<CategoryDto[]>(
                json: SelectedCategories,
                options: GetJsonSerializerOptions()
            );

            // If the length of selected categories is equal zero then return false.
            if (selectedCategories.Length == 0)
            {
                return Result<IEnumerable<CategoryDto>>.Failed();
            }

            return Result<IEnumerable<CategoryDto>>.Success(selectedCategories);
        }
        catch
        {
            return Result<IEnumerable<CategoryDto>>.Failed();
        }
    }

    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        lock (_lock)
        {
            if (Equals(_jsonSerializerOptions, null))
            {
                _jsonSerializerOptions = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    PropertyNameCaseInsensitive = true
                };
            }
        }

        return _jsonSerializerOptions;
    }

    public Art7Request MapToFeatureRequest(long updatedBy)
    {
        var request = new Art7Request
        {
            ComicId = ComicId,
            Title = Title,
            Introduction = Introduction,
            OriginId = OriginId,
            PublicLevel = PublicLevel,
            AllowComment = AllowComment,
            AuthorName = "Default",
            UpdatedBy = updatedBy,
            ArtworkStatus = ArtworkStatus.OnGoing,
        };

        // Set the thumbnail image file info if update is found.
        if (IsThumbnailUpdated())
        {
            const string thumbnailImageName = "thumbnail";
            var fileExtension = FormFileHelper.Instance.GetFileExtension(ThumbnailImageFile);

            request.ThumbnailFileInfo = new ImageFileInfo
            {
                FileId = ComicId.ToString(),
                FileName = $"{thumbnailImageName}.{fileExtension}",
                FileDataStream = ThumbnailImageFile.OpenReadStream(),
            };

            request.IsThumbnailUpdated = true;
        }

        // Set the artwork categories if the update is found.
        if (IsCategoriesUpdated)
        {
            request.ArtworkCategories = ArtworkCategories.Select(
                artworkCategory => new ArtworkCategory
                {
                    ArtworkId = ComicId,
                    CategoryId = long.Parse(artworkCategory.Id),
                }
            );

            request.IsCategoriesUpdated = true;
        }

        return request;
    }
}
