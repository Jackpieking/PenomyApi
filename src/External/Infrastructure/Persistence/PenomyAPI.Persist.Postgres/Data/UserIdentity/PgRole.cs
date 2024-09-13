using Microsoft.AspNetCore.Identity;
using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Persist.Postgres.Data.UserIdentity;

public sealed class PgRole : IdentityRole<long>, IEntity { }
