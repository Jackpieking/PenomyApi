using FileTypeChecker;
using FileTypeChecker.Abstracts;
using FileTypeChecker.Extensions;
using Microsoft.AspNetCore.Http;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles.VideoFileTypes;
using System.IO;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.IFormFiles.VideoFileValiationExtensions;

public static class VideoFileValidationExtension
{
    static VideoFileValidationExtension()
    {
        FileTypeValidator.RegisterCustomTypes(typeof(VideoFileValidationExtension).Assembly);
    }

    public static bool IsVideo(this IFormFile formFile)
    {
        var fileContent = formFile.OpenReadStream();

        var result = 
            fileContent.Is<FileTypeChecker.Types.Mp4>()
            || fileContent.Is<FileTypeChecker.Types.M4V>()
            || fileContent.Is<FileTypeChecker.Types.AudioVideoInterleaveVideoFormat>()
            || fileContent.Is<Mkv>();

        return result;
    }
}
