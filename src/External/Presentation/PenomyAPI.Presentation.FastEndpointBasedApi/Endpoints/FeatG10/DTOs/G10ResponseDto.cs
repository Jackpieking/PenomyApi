using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.DTOs
{
    public class G10ResponseDto
    {
        public List<ArtworkComment> ArtworkList { get; set; }
    }
    public class G10ResponseDtoObject
    {
        public Guid ComicId { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public int Favorite { get; set; }
        public string Thumbnail { get; set; }
    }
}
