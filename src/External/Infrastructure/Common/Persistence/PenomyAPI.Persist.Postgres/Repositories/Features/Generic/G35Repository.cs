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

    public G35Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
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

    public Task<bool> IsUserIdExistedAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserProfile>()
            .AnyAsync(
                user => user.UserId == userId,
                cancellationToken);
    }

    public Task<bool> IsCreatorIdExistedAsync(
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<CreatorProfile>()
            .AnyAsync(
                user => user.CreatorId == creatorId,
                cancellationToken);
    }

    public Task<UserProfile> GetUserProfileByIdAsync(
        long userId,
        bool isProfileOwner,
        CancellationToken cancellationToken)
    {
        var userProfileDbSet = _dbContext.Set<UserProfile>();

        // If the request is served for the profile owner, then return
        // additional information for the user.
        if (isProfileOwner)
        {
            return userProfileDbSet
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
            })
            .FirstOrDefaultAsync(cancellationToken);
        }

        return userProfileDbSet
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new UserProfile
            {
                UserId = profile.UserId,
                NickName = profile.NickName,
                AvatarUrl = profile.AvatarUrl,
                AboutMe = profile.AboutMe,
                RegisterAsCreator = profile.RegisterAsCreator,
                RegisteredAt = profile.RegisteredAt,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<UserProfile> GetUserProfileAsCreatorByIdAsync(
        long userId,
        bool isProfileOwner,
        CancellationToken cancellationToken)
    {
        var userProfileDbSet = _dbContext.Set<UserProfile>();

        // If the request is served for the profile owner, then return
        // additional information for the user.
        if (isProfileOwner)
        {
            return userProfileDbSet
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new UserProfile
            {
                UserId = profile.UserId,
                NickName = profile.NickName,
                AvatarUrl = profile.AvatarUrl,
                RegisterAsCreator = profile.RegisterAsCreator,
                TotalFollowedCreators = profile.TotalFollowedCreators,
                AboutMe = profile.AboutMe,
                RegisteredAt = profile.RegisteredAt,
                LastActiveAt = profile.LastActiveAt,
                // Creator detail section.
                CreatorProfile = new CreatorProfile
                {
                    RegisteredAt = profile.CreatorProfile.RegisteredAt,
                    TotalFollowers = profile.CreatorProfile.TotalFollowers,
                    TotalArtworks = profile.CreatorProfile.TotalArtworks,
                },
            })
            .FirstOrDefaultAsync(cancellationToken);
        }

        return userProfileDbSet
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new UserProfile
            {
                UserId = profile.UserId,
                NickName = profile.NickName,
                AvatarUrl = profile.AvatarUrl,
                RegisterAsCreator = profile.RegisterAsCreator,
                AboutMe = profile.AboutMe,
                RegisteredAt = profile.RegisteredAt,
                // Creator detail section.
                CreatorProfile = new CreatorProfile
                {
                    RegisteredAt = profile.CreatorProfile.RegisteredAt,
                    TotalFollowers = profile.CreatorProfile.TotalFollowers,
                    TotalArtworks = profile.CreatorProfile.TotalArtworks,
                },
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<UserProfile> GetCreatorProfileByIdAsync(
        long creatorId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Set<UserProfile>()
            .AsNoTracking()
            .Where(profile => profile.CreatorProfile.CreatorId == creatorId)
            .Select(profile => new UserProfile
            {
                UserId = profile.UserId,
                NickName = profile.NickName,
                AvatarUrl = profile.AvatarUrl,
                AboutMe = profile.AboutMe,
                RegisteredAt = profile.RegisteredAt,
                CreatorProfile = new CreatorProfile
                {
                    RegisteredAt = profile.CreatorProfile.RegisteredAt,
                    TotalArtworks = profile.CreatorProfile.TotalArtworks,
                    TotalFollowers = profile.CreatorProfile.TotalFollowers,
                },
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
