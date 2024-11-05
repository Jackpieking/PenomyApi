using Microsoft.EntityFrameworkCore;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Persist.Postgres.Data.UserIdentity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Persist.Postgres.Repositories.Features.Generic;

internal sealed class G35Repository : IG35Repository
{
    private readonly DbContext _dbContext;
    private readonly DbSet<UserProfile> _userProfileDbSet;

    public G35Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _userProfileDbSet = dbContext.Set<UserProfile>();
    }

    public Task<bool> IsRefreshTokenValidAsync(
        string refreshTokenId,
        string refreshTokenValue,
        long userId,
        CancellationToken cancellationToken)
    {
        return _dbContext
            .Set<PgUserToken>()
            .AnyAsync(
                token => token.UserId.Equals(userId)
                && token.LoginProvider.Equals(refreshTokenId)
                && token.Value.Equals(refreshTokenValue),
                cancellationToken);
    }

    public Task<bool> IsUserRegisteredAsCreatorByIdAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        return _userProfileDbSet.AnyAsync(
            user => user.UserId == userId
            && user.RegisterAsCreator,
            cancellationToken);
    }

    public Task<UserProfile> GetUserProfileByIdAsync(
        long userId,
        bool isCreator,
        CancellationToken cancellationToken)
    {
        // If the current user is creator, then also fetch their creator profile.
        if (isCreator)
        {
            return _userProfileDbSet
                .AsNoTracking()
                .Where(profile => profile.UserId == userId)
                .Select(profile => new UserProfile
                {
                    UserId = profile.UserId,
                    NickName = profile.NickName,
                    AvatarUrl = profile.AvatarUrl,
                    AboutMe = profile.AboutMe,
                    RegisterAsCreator = profile.RegisterAsCreator,
                    TotalFollowedCreators = profile.TotalFollowedCreators,
                    CreatorProfile = new CreatorProfile
                    {
                        RegisteredAt = profile.CreatorProfile.RegisteredAt,
                        TotalArtworks = profile.CreatorProfile.TotalArtworks,
                        TotalFollowers = profile.CreatorProfile.TotalFollowers,
                    },
                    RegisteredAt = profile.RegisteredAt,
                    LastActiveAt = profile.LastActiveAt,
                    UpdatedAt = profile.UpdatedAt,
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        // Otherwise, fetch the user profile only.
        return _userProfileDbSet
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new UserProfile
            {
                UserId = profile.UserId,
                NickName = profile.NickName,
                AvatarUrl = profile.AvatarUrl,
                AboutMe = profile.AboutMe,
                RegisterAsCreator = profile.RegisterAsCreator,
                TotalFollowedCreators = profile.TotalFollowedCreators,
                RegisteredAt = profile.RegisteredAt,
                LastActiveAt = profile.LastActiveAt,
                UpdatedAt = profile.UpdatedAt,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
