﻿using FileTypeChecker;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

public sealed class FormFileHelper : IFormFileHelper
{
    public static readonly string[] VALID_IMAGE_FILE_EXTENSIONS = ["png", "jpg", "jpeg"];

    private FormFileHelper() { }

    /// <summary>
    ///     The singleton instance of this <see cref="IFormFileHelper"/>
    ///     interface to support file validation.
    /// </summary>
    public static readonly IFormFileHelper Instance = new FormFileHelper();

    public string GetFileExtension(IFormFile formFile)
    {
        var fileMimeType = formFile.ContentType;
        var mimeTypeSplit = fileMimeType.Split('/');

        return mimeTypeSplit.LastOrDefault();
    }

    public bool HasValidExtension(IFormFile formFile, params string[] validFileExtensions)
    {
        // Return false when form file is null.
        if (Equals(formFile, null))
        {
            return false;
        }

        var fileExtension = GetFileExtension(formFile);

        return validFileExtensions.Any(
            predicate: validFileExtension => validFileExtension.Equals(
                value: fileExtension,
                comparisonType: System.StringComparison.OrdinalIgnoreCase));
    }

    public bool IsValidImageFile(IFormFile formFile)
    {
        // Check the byte signatures of the file, preventing
        // malicious users upload file that has been modified.
        return FileTypeValidator.IsImage(formFile.OpenReadStream());
    }
}