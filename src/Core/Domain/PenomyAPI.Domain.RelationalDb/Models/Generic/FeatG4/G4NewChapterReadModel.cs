using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG4;

public sealed class G4NewChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
