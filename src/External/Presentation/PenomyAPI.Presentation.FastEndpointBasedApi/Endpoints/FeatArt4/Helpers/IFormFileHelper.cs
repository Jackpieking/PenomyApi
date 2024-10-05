using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.Helpers
{
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
            var fileExtension = GetFileExtension(formFile);

            return validFileExtensions.Any(
                predicate: validFileExtension => validFileExtension.Equals(
                    value: fileExtension,
                    comparisonType: System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
