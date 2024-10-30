using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G31ARepository : IG31ARepository
{
    private readonly AppDbContext _context;

    public G31ARepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateRefreshTokenAsync(
        string oldTokenId,
        string newTokenId,
        CancellationToken ct
    )
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
                        .Where(token => token.LoginProvider.Equals(oldTokenId))
                        .ExecuteUpdateAsync(
                            prop => prop.SetProperty(token => token.LoginProvider, newTokenId),
                            ct
                        );

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

    public Task<bool> IsRefreshTokenFoundAsync(
        string refreshTokenId,
        string refreshTokenValue,
        CancellationToken ct
    )
    {
        return _context
            .Set<PgUserToken>()
            .AnyAsync(
                token =>
                    token.LoginProvider.Equals(refreshTokenId)
                    && token.Value.Equals(refreshTokenValue),
                ct
            );
    }

    public Task<bool> IsRefreshTokenExpiredAsync(string refreshTokenId, CancellationToken ct)
    {
        return _context
            .Set<PgUserToken>()
            .AnyAsync(
                token =>
                    token.LoginProvider.Equals(refreshTokenId) && token.ExpiredAt < DateTime.UtcNow,
                ct
            );
    }
}
