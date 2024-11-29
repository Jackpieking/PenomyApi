namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG63;

public sealed class G63CreatorProfileReadModel
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string AvatarUrl { get; set; }

    public long TotalFollowers { get; set; }

    public int TotalArtworks { get; set; }
}
