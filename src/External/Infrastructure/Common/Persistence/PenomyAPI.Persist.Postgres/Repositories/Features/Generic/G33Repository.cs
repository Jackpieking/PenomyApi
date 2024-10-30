using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G33Repository : IG33Repository
{
    private readonly AppDbContext _context;

    public G33Repository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsRefreshTokenFoundByIdAsync(string refreshTokenId, CancellationToken ct)
    {
        return _context
            .Set<PgUserToken>()
            .AnyAsync(token => token.LoginProvider.Equals(refreshTokenId), ct);
    }

    public async Task<bool> RemoveRefreshTokenAsync(string refreshTokenId, CancellationToken ct)
    {
        var dbResult = true;

        await _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await _context.Database.BeginTransactionAsync(ct);

                try
                {
                    await _context
                        .Set<PgUserToken>()
                        .Where(token => token.LoginProvider.Equals(refreshTokenId))
                        .ExecuteDeleteAsync(ct);

                    await dbTransaction.CommitAsync(ct);
                }
                catch
                {
                    await dbTransaction.RollbackAsync(ct);

                    dbResult = false;
                }
            });

        return dbResult;
    }
}
