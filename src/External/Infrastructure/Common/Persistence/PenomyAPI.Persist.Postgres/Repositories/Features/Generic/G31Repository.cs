using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G31Repository : IG31Repository
{
    private readonly DbContext _dbContext;
    private readonly Lazy<UserManager<PgUser>> _userManager;
    private readonly Lazy<SignInManager<PgUser>> _signInManager;

    public G31Repository(
        DbContext dbContext,
        Lazy<UserManager<PgUser>> userManager,
        Lazy<SignInManager<PgUser>> signInManager
    )
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(
        bool isPasswordCorrect,
        bool isUserTemporarilyLockedOut,
        long userIdOfUserHasBeenValidated
    )> CheckPasswordSignInAsync(string email, string password, CancellationToken ct)
    {
        var foundUser = await _userManager.Value.FindByEmailAsync(email);

        var passwordValidatingResult = await _signInManager.Value.CheckPasswordSignInAsync(
            foundUser,
            password,
            lockoutOnFailure: true
        );

        return (
            passwordValidatingResult.Succeeded,
            passwordValidatingResult.IsLockedOut,
            foundUser.Id
        );
    }

    public async Task<bool> CreateRefreshTokenCommandAsync(
        UserToken refreshToken,
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
                        LoginProvider = refreshToken.LoginProvider,
                        ExpiredAt = refreshToken.ExpiredAt,
                        UserId = refreshToken.UserId,
                        Value = refreshToken.Value,
                        Name = refreshToken.Name
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

    public Task<UserProfile> GetUserProfileAsync(long userId, CancellationToken ct)
    {
        return _dbContext
            .Set<UserProfile>()
            .Where(user => user.UserId == userId)
            .Select(user => new UserProfile
            {
                NickName = user.NickName,
                AvatarUrl = user.AvatarUrl
            })
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> IsUserFoundByEmailAsync(string email, CancellationToken ct)
    {
        var upperCaseEmail = email.ToUpper();

        return _dbContext
            .Set<PgUser>()
            .AnyAsync(user => user.NormalizedEmail.Equals(upperCaseEmail), ct);
    }
}
