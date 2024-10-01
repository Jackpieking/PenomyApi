using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.DTOs
{
    public class G3ResponseDto
    {
        public List<Artwork> ArtworkList { get; set; }
    }
    public class FeatG3ResponseDtoObject
    {
        public Guid ComicId { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public int Favorite { get; set; }
        public string Thumbnail { get; set; }
    }
}
