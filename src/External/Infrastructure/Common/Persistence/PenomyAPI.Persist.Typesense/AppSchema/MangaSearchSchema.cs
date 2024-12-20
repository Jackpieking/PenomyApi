using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PenomyAPI.Persist.Typesense.AppSchema;

public sealed class MangaSearchSchema
{
    [JsonPropertyName(Metadata.FieldTitle.MangaId)]
    public string MangaId { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaName)]
    public string MangaName { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaDescription)]
    public string MangaDescription { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaCategories)]
    public IEnumerable<string> MangaCategories { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaAvatar)]
    public string MangaAvatar { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.ArtworkType)]
    public int ArtworkType { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaNumberOfStars)]
    public long MangaNumberOfStars { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaNumberOfFollowers)]
    public long MangaNumberOfFollowers { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.Embedding)]
    public float[] Embedding { get; set; }

    public sealed class Metadata
    {
        public const string SchemaName = "MangaSearch";

        public sealed class FieldTitle
        {
            public const string MangaId = "id";

            public const string MangaName = "MangaName";

            public const string MangaDescription = "MangaDescription";

            public const string MangaCategories = "MangaCategories";

            public const string MangaAvatar = "MangaAvatar";

            public const string ArtworkType = "ArtworkType";

            public const string MangaNumberOfStars = "MangaNumberOfStars";

            public const string MangaNumberOfFollowers = "MangaNumberOfFollowers";

            public const string Embedding = "embedding";
        }
    }
}
