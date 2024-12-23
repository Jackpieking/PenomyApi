using System;
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

internal sealed class G1Repository : IG1Repository
{
    private readonly AppDbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G1Repository(AppDbContext dbContext, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<bool> AddNewUserToDatabaseAsync(
        User newUser,
        UserProfile newUserProfile,
        string password,
        CancellationToken ct
    )
    {
        var dbResult = true;

        await _dbContext
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    var newPgUser = new PgUser
                    {
                        Id = newUser.Id,
                        UserName = newUser.UserName,
                        Email = newUser.Email,
                        EmailConfirmed = newUser.EmailConfirmed,
                    };

                    var result = await _userManager.Value.CreateAsync(
                        user: newPgUser,
                        password: password
                    );

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    await _dbContext.Set<UserProfile>().AddAsync(newUserProfile);

                    await _dbContext.SaveChangesAsync(cancellationToken: ct);

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
        var upperEmail = email.ToUpper();

        return _dbContext
            .Set<PgUser>()
            .AnyAsync(user => user.NormalizedEmail.Equals(upperEmail), ct);
    }

    public async Task<bool> ValidatePasswordAsync(User newUser, string newPassword)
    {
        IdentityResult result = default;

        foreach (var validator in _userManager.Value.PasswordValidators)
        {
            result = await validator.ValidateAsync(
                _userManager.Value,
                new() { Id = newUser.Id },
                newPassword
            );
        }

        if (Equals(result, default))
        {
            return false;
        }

        return result.Succeeded;
    }
}
