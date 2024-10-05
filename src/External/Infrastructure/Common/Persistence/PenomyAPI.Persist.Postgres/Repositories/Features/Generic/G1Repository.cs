using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G1Repository : IG1Repository
{
    private readonly DbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G1Repository(DbContext dbContext, Lazy<UserManager<PgUser>> userManager)
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
                    var result = await _userManager.Value.CreateAsync(
                        user: new()
                        {
                            Id = newUser.Id,
                            UserName = newUser.UserName,
                            Email = newUser.Email,
                        },
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

    public Task<bool> IsUserFoundByEmailQueryAsync(string email, CancellationToken ct)
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
                manager: _userManager.Value,
                user: new() { Id = newUser.Id },
                password: newPassword
            );
        }

        if (Equals(objA: result, objB: default))
        {
            return false;
        }

        return result.Succeeded;
    }
}
