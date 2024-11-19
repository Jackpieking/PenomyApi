using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG6.DTOs;

public class G6ResponseDto
{
    public List<ArtworkDto> Result { get; set; }
}

public class ArtworkDto
{
    public long Id { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public string ThumbnailUrl { get; set; }
    public double StarRates { get; set; }
}
