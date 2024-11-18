using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG3;

public sealed class G3NewChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
