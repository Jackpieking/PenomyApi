using Microsoft.AspNetCore.Http;
using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatArt4.DTOs
{
    public class FeatArt4RequestDto
    {
        public string Title { get; set; }

        public IFormFile ThumbnailImageFile { get; set; }

        public long OriginId { get; set; }

        public string Introduction { get; set; }

        public string SelectedCategories { get; set; }

        public ArtworkPublicLevel PublicLevel { get; set; }

        public bool AllowComment { get; set; }
    }
}
