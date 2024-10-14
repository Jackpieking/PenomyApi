using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkViewStatistic : EntityWithId<long>
{
    public long ArtworkId { get; set; }

    public long TotalViews { get; set; }

    /// <summary>
    ///     The datetime from that this record is accumulated and persisted.
    /// </summary>
    public DateTime From { get; set; }

    /// <summary>
    ///     The datetime to that this record is finished in accumulation and persisted.
    /// </summary>
    public DateTime To { get; set; }
}
