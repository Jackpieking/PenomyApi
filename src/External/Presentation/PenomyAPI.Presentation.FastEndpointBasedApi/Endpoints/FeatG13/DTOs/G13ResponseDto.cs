using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG13.DTOs
{
    public class G13ResponseDto
    {
        public IEnumerable<G13ResponseDtoObject> ArtworkList { get; set; }
    }
    public class G13ResponseDtoObject
    {
        public long ArtworkId { get; set; }
        public string Title { get; set; }
        public string Supplier { get; set; }
        public double Rating { get; set; }
        public long Favorite { get; set; }
        public string FlagUrl { get; set; }
        public string Thumbnail { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
