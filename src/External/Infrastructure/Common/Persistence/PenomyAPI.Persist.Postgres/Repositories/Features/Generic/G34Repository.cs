using System;
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

    public async Task<string> GenerateResetPasswordTokenAsync(string email, CancellationToken ct)
    {
        var foundUser = await _userManager.Value.FindByEmailAsync(email);

        return await _userManager.Value.GeneratePasswordResetTokenAsync(foundUser);
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
