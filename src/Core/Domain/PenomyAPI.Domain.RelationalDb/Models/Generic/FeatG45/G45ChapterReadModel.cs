using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG45;

public sealed class G45ChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
