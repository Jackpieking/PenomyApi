namespace PenomyAPI.Domain.RelationalDb.Models.ArtworkCreation.FeatArt3;

public class Art3CheckDeletedItemReadModel
{
    public int TotalDeletedComics { get; set; }

    public int TotalDeletedAnimes { get; set; }

    public bool HasDeletedItems => TotalDeletedAnimes > 0 || TotalDeletedComics > 0;

    public static readonly Art3CheckDeletedItemReadModel Empty = new()
    {
        TotalDeletedComics = 0,
        TotalDeletedAnimes = 0,
    };
}
