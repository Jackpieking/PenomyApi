using System;

namespace PenomyAPI.Domain.RelationalDb.Models.Generic.FeatG25;

public sealed class G25ViewHistoryChapterReadModel
{
    public long Id { get; set; }

    public int UploadOrder { get; set; }

    public DateTime ViewedAt { get; set; }
}
