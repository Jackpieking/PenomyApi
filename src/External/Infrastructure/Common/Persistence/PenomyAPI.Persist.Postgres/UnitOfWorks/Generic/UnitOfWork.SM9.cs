﻿using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

namespace PenomyAPI.Persist.Postgres.UnitOfWorks;

public sealed partial class UnitOfWork
{
    private ISM9Repository _SM9Repository;

    public ISM9Repository SM9Repository
    {
        get
        {
            _SM9Repository ??= new SM9Repository(_dbContext);

            return _SM9Repository;
        }
    }
}