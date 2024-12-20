using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.Entities.Base;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Data.UserIdentity;

public sealed class PgRole : IdentityRole<long>, IEntity
{
    public static PgRole MapFrom(Role role)
    {
        return new()
        {
            Id = role.Id,
            Name = role.Name,
            NormalizedName = role.NormalizedName,
            ConcurrencyStamp = role.ConcurrencyStamp,
        };
    }
}
