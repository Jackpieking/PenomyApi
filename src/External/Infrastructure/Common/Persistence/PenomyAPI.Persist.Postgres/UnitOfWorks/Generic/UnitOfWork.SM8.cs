﻿using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM8Repository _SM8Repository;

    public ISM8Repository SM8Repository
    {
        get
        {
            _SM8Repository ??= new SM8Repository(_dbContext);

            return _SM8Repository;
        }
    }
}