﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Helpers
{
    internal static class RepositoryHelper
    {
        public static IExecutionStrategy CreateExecutionStrategy(DbContext dbContext)
        {
            return dbContext.Database.CreateExecutionStrategy();
        }

        public static Task<IDbContextTransaction> CreateTransactionAsync(
            DbContext dbContext,
            CancellationToken ct)
        {
            return dbContext.Database.BeginTransactionAsync(ct);
        }
    }
}
