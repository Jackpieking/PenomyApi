using System.Text.Json.Serialization;

namespace PenomyAPI.Persist.Typesense.AppSchema;

public sealed class MangaSearchSchema
{
    [JsonPropertyName(Metadata.FieldTitle.MangaId)]
    public string MangaId { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaName)]
    public string MangaName { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaAvatar)]
    public string MangaAvatar { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaNumberOfStars)]
    public long MangaNumberOfStars { get; set; }

    [JsonPropertyName(Metadata.FieldTitle.MangaNumberOfFollowers)]
    public long MangaNumberOfFollowers { get; set; }

    public sealed class Metadata
    {
        public const string SchemaName = "MangaSearch";

        public sealed class FieldTitle
        {
            public const string MangaId = "id";

            public const string MangaName = "MangaName";

            public const string MangaAvatar = "MangaAvatar";

            public const string MangaNumberOfStars = "MangaNumberOfStars";

            public const string MangaNumberOfFollowers = "MangaNumberOfFollowers";
        }
    }
}
