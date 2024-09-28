using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG7.DTOs;

public class G7ResponseDto
{
    public List<ArtworkDto> Result { get; set; }
}

public class ArtworkDto
{
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public string ThumbnailUrl { get; set; }
    public byte StarRates { get; set; }
}
