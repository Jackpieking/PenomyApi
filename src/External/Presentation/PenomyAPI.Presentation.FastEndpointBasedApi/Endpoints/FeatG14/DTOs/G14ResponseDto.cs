using System.Collections.Generic;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG14.DTOs;

public class G14ResponseDto
{
    public List<ArtworkDto> Result { get; set; }
}

public class ArtworkDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool HasSeries { get; set; }
    public string AuthorName { get; set; }
    public string CountryName { get; set; }
    public List<string> Categories { get; set; }
    public string ArtworkStatus { get; set; }
    public string SeriesName { get; set; }
    public long ViewCount { get; set; }
    public long FavoriteCount { get; set; }
    public double StarRates { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Introduction { get; set; }
    public long CommentCount { get; set; }
}
