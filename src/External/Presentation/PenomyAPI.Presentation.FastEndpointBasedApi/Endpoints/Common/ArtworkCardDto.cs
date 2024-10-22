using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Common
{
    public class ArtworkCardDto
    {
        public IEnumerable<ArtworkDto> Artworks { get; set; }
    }

    public class ArtworkDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string AuthorName { get; set; }
        public string ThumbnailUrl { get; set; }
        public long TotalFavorites { get; set; }
        public double AverageStarRate { get; set; }
        public string OriginUrl { get; set; }
        public IEnumerable<ChapterDto> Chapters { get; set; }
    }

    public class ChapterDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int UploadOrder { get; set; }
        public DateTime Time { get; set; }
    }

}
