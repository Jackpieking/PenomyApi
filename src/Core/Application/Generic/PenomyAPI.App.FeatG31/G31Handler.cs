using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG31;

public sealed class G31Handler : IFeatureHandler<G31Request, G31Response>
{
    private readonly Lazy<IRefreshTokenHandler> _refreshTokenHandler;
    private readonly Lazy<IAccessTokenHandler> _accessTokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;
    private readonly IG31Repository _g31Repository;

    public G31Handler(
        Lazy<IRefreshTokenHandler> refreshTokenHandler,
        Lazy<IAccessTokenHandler> accessTokenHandler,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator,
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _refreshTokenHandler = refreshTokenHandler;
        _accessTokenHandler = accessTokenHandler;
        _snowflakeIdGenerator = snowflakeIdGenerator;
        _g31Repository = unitOfWork.Value.G31Repository;
    }

    public async Task<G31Response> ExecuteAsync(G31Request request, CancellationToken ct)
    {
        // Is user found by email.
        var isUserFound = await _g31Repository.IsUserFoundByEmailAsync(request.Email, ct);

        // User with email does not exist.
        if (!isUserFound)
        {
            return new() { StatusCode = G31ResponseStatusCode.USER_NOT_FOUND };
        }

        // Does password belong to user.
        var (isPasswordCorrect, isUserTemporarilyLockedOut, userIdOfUserHasBeenValidated) =
            await _g31Repository.CheckPasswordSignInAsync(request.Email, request.Password, ct);

        if (!isPasswordCorrect)
        {
            // User password is uncorrect and number of login attempts is exceeded.
            if (isUserTemporarilyLockedOut)
            {
                return new() { StatusCode = G31ResponseStatusCode.TEMPORARY_BANNED };
            }

            // User password is uncorrect still can try to login again.
            return new() { StatusCode = G31ResponseStatusCode.PASSWORD_IS_INCORRECT };
        }

        // Generate token id.
        var tokenId = _snowflakeIdGenerator.Value.Get().ToString();

        // Create new refresh token.
        var newRefreshToken = InitNewRefreshToken(
            userIdOfUserHasBeenValidated,
            tokenId,
            request.RememberMe
        );

        // Add new refresh token to the database.
        var dbResult = await _g31Repository.CreateRefreshTokenCommandAsync(newRefreshToken, ct);

        // Cannot add new refresh token to the database.
        if (!dbResult)
        {
            return new() { StatusCode = G31ResponseStatusCode.DATABASE_ERROR };
        }

        // Get additional user information.
        var foundUserProfile = await _g31Repository.GetUserProfileAsync(
            userIdOfUserHasBeenValidated,
            ct
        );

        // Generate access token.
        var newAccessToken = _accessTokenHandler.Value.Generate(
            claims: [new("jti", tokenId), new("sub", userIdOfUserHasBeenValidated.ToString())],
            additionalSecondsFromNow: 10 * 60 // 10 minutes
        );

        return new()
        {
            StatusCode = G31ResponseStatusCode.SUCCESS,
            Body = new()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Value,
                User = new()
                {
                    Id = userIdOfUserHasBeenValidated,
                    AvatarUrl = foundUserProfile.AvatarUrl,
                    Nickname = foundUserProfile.NickName
                }
            }
        };
    }

    private UserToken InitNewRefreshToken(long userId, string tokenId, bool isRememberMe)
    {
        return new()
        {
            LoginProvider = tokenId,
            ExpiredAt = isRememberMe
                ? DateTime.UtcNow.AddDays(value: 15)
                : DateTime.UtcNow.AddDays(value: 3),
            UserId = userId,
            Value = _refreshTokenHandler.Value.Generate(length: 15),
            Name = "AppUserRefreshToken"
        };
    }
}
