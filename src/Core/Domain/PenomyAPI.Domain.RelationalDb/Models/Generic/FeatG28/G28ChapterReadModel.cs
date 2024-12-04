using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG28;

public class G28ChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime PublishedAt { get; set; }
}
