using System;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.Typs2;

public class Typs2HttpResponse
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
        public string MangaId { get; init; }

        public string MangaName { get; init; }

        public string MangaAvatar { get; init; }

        public int ArtworkType { get; init; }

        public string MangaDescription { get; init; }

        public long MangaNumberOfStars { get; init; }

        public long MangaNumberOfFollowers { get; init; }
    }
}
