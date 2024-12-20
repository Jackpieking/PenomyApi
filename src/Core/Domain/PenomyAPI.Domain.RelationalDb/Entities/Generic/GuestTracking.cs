using System;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Generic;

public sealed class GuestTracking : IEntity
{
    public long GuestId { get; set; }

    public DateTime LastActiveAt { get; set; }
}
