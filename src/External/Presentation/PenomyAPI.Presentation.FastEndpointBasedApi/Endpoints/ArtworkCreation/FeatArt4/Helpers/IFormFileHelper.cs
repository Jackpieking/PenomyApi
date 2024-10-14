using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.ArtworkCreation.FeatArt4.Helpers;

public static class IFormFileHelper
{
    public static string GetFileExtension(IFormFile formFile)
    {
        // Add 1 to not include the dot in the extracted extension.
        var lastIndexOfDot = formFile.FileName.LastIndexOf('.') + 1;

        return formFile.FileName[lastIndexOfDot..];
    }

    public static bool IsValidFileExtension(IFormFile formFile, string[] validFileExtensions)
    {
        // Return false when form file is null.
        if (Equals(formFile, null))
        {
            return false;
        }

        var fileExtension = GetFileExtension(formFile);

        return validFileExtensions.Any(predicate: validFileExtension =>
            validFileExtension.Equals(
                value: fileExtension,
                comparisonType: System.StringComparison.OrdinalIgnoreCase
            )
        );
    }
}
