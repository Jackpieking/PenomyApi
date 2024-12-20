using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG48;

public sealed class G48ChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
