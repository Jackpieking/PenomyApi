using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs3;

public class Typs3HttpResponse
{
    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        );

    public object Body { get; init; } = new();

    public object Errors { get; init; }

    public sealed class MangaSearchResult
    {
        public string MangaId { get; set; }

        public string MangaName { get; set; }

        public string MangaAvatar { get; set; }

        public int ArtworkType { get; set; }

        public long MangaNumberOfStars { get; set; }

        public long MangaNumberOfFollowers { get; set; }
    }
}
