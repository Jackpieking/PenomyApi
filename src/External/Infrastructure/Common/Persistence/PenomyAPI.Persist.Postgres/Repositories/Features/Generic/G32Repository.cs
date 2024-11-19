using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

public sealed class G32Repository : IG32Repository
{
    private readonly AppDbContext _context;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G32Repository(AppDbContext context, Lazy<UserManager<PgUser>> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> AddNewUserAndRefreshTokenToDatabaseAsync(
        User user,
        UserProfile userProfile,
        UserToken refreshToken,
        CancellationToken ct
    )
    {
        var dbResult = true;

        await _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var dbTransaction = await _context.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    var newPgUser = new PgUser
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                    };

                    // Add user.
                    var result = await _userManager.Value.CreateAsync(newPgUser);

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    // Add user profile.
                    await _context.Set<UserProfile>().AddAsync(userProfile);

                    // Add refresh token.
                    await _context
                        .Set<PgUserToken>()
                        .AddAsync(
                            new()
                            {
                                LoginProvider = refreshToken.LoginProvider,
                                ExpiredAt = refreshToken.ExpiredAt,
                                UserId = refreshToken.UserId,
                                Value = refreshToken.Value,
                                Name = refreshToken.Name
                            },
                            ct
                        );

                    await _context.SaveChangesAsync(cancellationToken: ct);

                    await dbTransaction.CommitAsync(cancellationToken: ct);
                }
                catch
                {
                    await dbTransaction.RollbackAsync(cancellationToken: ct);

                    dbResult = false;
                }
            });

        return dbResult;
    }

    public Task<bool> IsUserFoundByEmailAsync(string email, CancellationToken ct)
    {
        var upperCaseEmail = email.ToUpper();

        return _context
            .Set<PgUser>()
            .AnyAsync(user => user.NormalizedEmail.Equals(upperCaseEmail), ct);
    }

    public async Task<bool> CreateRefreshTokenCommandAsync(
        UserToken refreshToken,
        CancellationToken ct
    )
    {
        try
        {
            await _context
                .Set<PgUserToken>()
                .AddAsync(
                    new()
                    {
                        LoginProvider = refreshToken.LoginProvider,
                        ExpiredAt = refreshToken.ExpiredAt,
                        UserId = refreshToken.UserId,
                        Value = refreshToken.Value,
                        Name = refreshToken.Name
                    },
                    ct
                );

            await _context.SaveChangesAsync(ct);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public Task<long> GetCurrentUserIdAsync(string email, CancellationToken ct)
    {
        var upperCaseEmail = email.ToUpper();

        return _context
            .Set<PgUser>()
            .Where(user => user.NormalizedEmail.Equals(upperCaseEmail))
            .Select(user => user.Id)
            .FirstOrDefaultAsync();
    }
}
