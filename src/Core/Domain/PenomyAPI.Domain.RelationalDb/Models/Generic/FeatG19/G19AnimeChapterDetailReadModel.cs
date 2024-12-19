namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG19;

public sealed class G19AnimeChapterDetailReadModel
{
    public long Id { get; set; }

    public long ArtworkId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int UploadOrder { get; set; }

    public bool AllowComment { get; set; }

    public long CreatedBy { get; set; }

    public string ChapterVideoUrl { get; set; }

    public long TotalViews { get; set; }
}
