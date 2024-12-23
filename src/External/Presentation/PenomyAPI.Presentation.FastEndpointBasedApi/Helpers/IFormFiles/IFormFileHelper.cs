﻿using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles;

public interface IFormFileHelper
{
    /// <summary>
    ///     Get the file extension of the input <paramref name="formFile"/>.
    /// </summary>
    /// <param name="formFile">
    ///     The form file to get the extension.
    /// </param>
    /// <returns>
    ///     The extension of the input <paramref name="formFile"/>.
    /// </returns>
    string GetFileExtension(IFormFile formFile);

    /// <summary>
    ///     Get the name of the input <paramref name="formFile"/> without its extension part.
    /// </summary>
    /// <param name="formFile">
    ///     The form file to get the filename.
    /// </param>
    /// <returns>
    ///     The filename without extension of the input <paramref name="formFile"/>.
    /// </returns>
    string GetFileNameWithoutExtension(IFormFile formFile);

    /// <summary>
    ///     Check if the input <paramref name="formFile"/> is actually
    ///     an image file or not, preventing from user to upload
    ///     malicious file to the server.
    /// </summary>
    /// <param name="formFile">
    ///     The form file to check the validity.
    /// </param>
    /// <returns>
    ///     The result (<see langword="bool"/>) after checking the file.
    /// </returns>
    bool IsValidImageFile(IFormFile formFile);

    /// <summary>
    ///     Check if the input <paramref name="formFile"/> is actually
    ///     an video file or not, preventing from user to upload
    ///     malicious file to the server.
    /// </summary>
    /// <param name="formFile">
    ///     The form file to check the validity.
    /// </param>
    /// <returns>
    ///     The result (<see langword="bool"/>) after checking the file.
    /// </returns>
    bool IsValidVideoFile(IFormFile formFile);

    /// <summary>
    ///     Check if the input file has extension that
    ///     includes in the <paramref name="validExtensions"/> or not.
    /// </summary>
    /// <param name="formFile">
    ///     The form file to check the validity.
    /// </param>
    /// <param name="validExtensions">
    ///     The list of valid file extensions to check.
    /// </param>
    /// <returns>
    ///     The result (<see langword="bool"/>) after checking the file.
    /// </returns>
    bool HasValidExtension(
        IFormFile formFile,
        params string[] validExtensions);
}
