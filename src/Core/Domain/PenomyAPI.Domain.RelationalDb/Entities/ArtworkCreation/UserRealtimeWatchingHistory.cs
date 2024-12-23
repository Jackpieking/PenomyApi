﻿using PenomyAPI.Domain.RelationalDb.Entities.Base;
using System;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class UserRealtimeWatchingHistory : EntityWithId<long>
{
    public long UserId { get; set; }

    public long ArtworkId { get; set; }

    public long ChapterId { get; set; }

    public DateTime WatchedAt { get; set; }

    #region Navigation
    public Artwork Artwork { get; set; }

    public ArtworkChapter Chapter { get; set; }
    #endregion

    #region MetaData
    public static class MetaData { }
    #endregion
}
