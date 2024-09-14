using System;
using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Persist.Postgres.Data.UserIdentity;

public sealed class PgUserToken : IdentityUserToken<long>, IEntity
{
    public long Id { get; set; }

    public DateTime ExpiredAt { get; set; }
}
