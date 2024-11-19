using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.DbContexts;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G34Repository : IG34Repository
{
    private readonly AppDbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;

    public G34Repository(AppDbContext dbContext, Lazy<UserManager<PgUser>> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public Task<long> GetUserIdByEmailAsync(string email, CancellationToken ct)
    {
        var upperEmail = email.ToUpper();

        return _dbContext
            .Set<PgUser>()
            .Where(user => user.NormalizedEmail.Equals(upperEmail))
            .Select(user => user.Id)
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> IsTokenFoundByTokenIdAsync(string tokenId, CancellationToken ct)
    {
        return _dbContext
            .Set<PgUserToken>()
            .AnyAsync(token => token.LoginProvider.Equals(tokenId), ct);
    }

    public async Task<bool> SaveResetPasswordTokenMetadataAsync(
        UserToken preResetPasswordToken,
        CancellationToken ct
    )
    {
        try
        {
            await _dbContext
                .Set<PgUserToken>()
                .AddAsync(
                    new()
                    {
                        LoginProvider = preResetPasswordToken.LoginProvider,
                        ExpiredAt = preResetPasswordToken.ExpiredAt,
                        UserId = preResetPasswordToken.UserId,
                        Value = preResetPasswordToken.Value,
                        Name = preResetPasswordToken.Name
                    },
                    ct
                );

            await _dbContext.SaveChangesAsync(ct);
        }
        catch
        {
            return false;
        }

        return true;
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

    public async Task<bool> UpdatePasswordAsync(
        long userId,
        string newPassword,
        string resetPasswordTokenId,
        CancellationToken ct
    )
    {
        var dbResult = false;

        await _dbContext
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(ct);

                try
                {
                    await _dbContext
                        .Set<PgUserToken>()
                        .Where(token => token.LoginProvider.Equals(resetPasswordTokenId))
                        .ExecuteDeleteAsync(ct);

                    var foundUser = await _userManager.Value.FindByIdAsync(userId.ToString());

                    var resetPasswordToken =
                        await _userManager.Value.GeneratePasswordResetTokenAsync(foundUser);

                    var result = await _userManager.Value.ResetPasswordAsync(
                        foundUser,
                        resetPasswordToken,
                        newPassword
                    );

                    if (!result.Succeeded)
                    {
                        throw new DbUpdateConcurrencyException();
                    }

                    await dbTransaction.CommitAsync(ct);

                    dbResult = true;
                }
                catch
                {
                    await dbTransaction.RollbackAsync(ct);
                }
            });

        return dbResult;
    }

    public Task<string> GetUserEmailByUserIdAsync(long userId, CancellationToken ct)
    {
        return _dbContext
            .Set<PgUser>()
            .Where(user => user.Id == userId)
            .Select(user => user.Email)
            .FirstOrDefaultAsync(ct);
    }
}
