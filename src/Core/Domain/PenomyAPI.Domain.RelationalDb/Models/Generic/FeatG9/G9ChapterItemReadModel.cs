namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG9;

public sealed class G9ChapterItemReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public string ThumbnailUrl { get; set; }

    public string ChapterName { get; set; }
}
