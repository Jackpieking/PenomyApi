using System;
using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G28.DTOs
{
    public class G28ResponseDto
    {
        public IEnumerable<G28ResponseDtoObject> ArtworkList { get; set; }
    }

    public class G28ResponseDtoObject
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
