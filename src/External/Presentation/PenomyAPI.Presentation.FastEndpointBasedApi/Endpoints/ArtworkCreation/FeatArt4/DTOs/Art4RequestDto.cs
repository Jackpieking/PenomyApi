using Microsoft.AspNetCore.Http;
using PenomyAPI.App.Common.FileServices.Models;
using PenomyAPI.App.Common.Models.Common;
using PenomyAPI.App.FeatArt4;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.Common.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.DTOs
{
    public class Art4RequestDto
    {
        private static readonly string[] _validFileExtensions = ["jpg", "png", "jpeg"];

        /// <summary>
        ///     A singleton instance of json serializer
        ///     options to reuse for next time.
        /// </summary>
        private static JsonSerializerOptions _jsonSerializerOptions;
        private static object _lock = new();

        public string Title { get; set; }

        [Required]
        public IFormFile ThumbnailImageFile { get; set; }

        public long OriginId { get; set; }

        public string Introduction { get; set; }

        /// <summary>
        ///     This property is store the json value with
        ///     schema as an array of <see cref="CategoryDto"/>.
        /// </summary>
        public string SelectedCategories { get; set; }

        public IEnumerable<CategoryDto> ArtworkCategories { get; private set; }

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

        public bool IsValidThumbnailImageFileInput()
        {
            var isValidFileExtension = IFormFileHelper.IsValidFileExtension(
                formFile: ThumbnailImageFile,
                validFileExtensions: _validFileExtensions);

            if (!isValidFileExtension)
            {
                return false;
            }

            return true;
        }

        private Result<IEnumerable<CategoryDto>> DeserializedSelectedCategories()
        {
            try
            {
                var selectedCategories = JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(
                    json: SelectedCategories,
                    options: GetJsonSerializerOptions());

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

        public Art4Request MapToFeatureRequest(long comicId, long createdBy)
        {
            const string thumbnailImageName = "thumbnail";
            var fileExtension = IFormFileHelper.GetFileExtension(ThumbnailImageFile);

            return new Art4Request
            {
                ComicId = comicId,
                Title = Title,
                Introduction = Introduction,
                OriginId = OriginId,
                PublicLevel = PublicLevel,
                AllowComment = AllowComment,
                ArtworkCategories = ArtworkCategories.Select(category => new ArtworkCategory
                {
                    ArtworkId = comicId,
                    CategoryId = long.Parse(category.Id),
                }),
                AuthorName = "Default",
                CreatedBy = createdBy,
                ThumbnailFileInfo = new ImageFileInfo
                {
                    FileId = comicId.ToString(),
                    FileName = $"{thumbnailImageName}.{fileExtension}",
                    FileDataStream = ThumbnailImageFile.OpenReadStream(),
                }
            };
        }
    }
}
