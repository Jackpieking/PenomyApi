using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.DTOs
{
    public class G8ResponseDto
    {
        public List<ArtworkChapterDto> Result { get; set; }
    }

    public class ArtworkChapterDto
    {
        public long Id { get; set; }
        public int UploadOrder { get; set; }
        public string ChapterName { get; set; }
        public DateTime CreatedTime { get; set; }
        public long ViewCount { get; set; }
        public long FavoriteCount { get; set; }
        public long CommentCount { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
