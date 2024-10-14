using PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;
using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs
{
    public class G4ResponseDto
    {
        public string Category { get; set; }
        public IEnumerable<FeatG4ResponseDtoObject> ArtworkList { get; set; }
    }
    public class FeatG4ResponseDtoObject
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
