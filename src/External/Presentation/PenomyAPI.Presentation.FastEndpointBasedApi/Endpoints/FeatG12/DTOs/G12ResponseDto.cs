using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG12.DTOs
{
    public class G12ResponseDto
    {
        public string Category { get; set; }
        public IEnumerable<FeatG12ResponseDtoObject> ArtworkList { get; set; }
    }
    public class FeatG12ResponseDtoObject
    {
        public string CategoryName { get; set; }
        public long ArtworkId { get; set; }
        public string Title { get; set; }
        public string Supplier { get; set; }
        public double Rating { get; set; }
        public long Favorite { get; set; }
        public string FlagUrl { get; set; }
        public string Thumbnail { get; set; }
    }
}
