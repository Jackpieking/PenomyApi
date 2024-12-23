﻿using PenomyAPI.Domain.RelationalDb.Repositories.Features.SocialMedia;
using PenomyAPI.Persist.Postgres.Repositories.Features.SocialMedia;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM38Repository _SM38Repository;

    public ISM38Repository SM38Repository
    {
        get
        {
            _SM38Repository ??= new SM38Repository(_dbContext);

            return _SM38Repository;
        }
    }
}
