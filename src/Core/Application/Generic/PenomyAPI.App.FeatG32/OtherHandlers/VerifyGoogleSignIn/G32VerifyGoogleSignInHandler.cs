using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.App.FeatG32.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG32.OtherHandlers.VerifyGoogleSignIn;

public sealed class G32VerifyGoogleSignInHandler
    : IFeatureHandler<G32VerifyGoogleSignInRequest, G32VerifyGoogleSignInResponse>
{
    private readonly Lazy<IRefreshTokenHandler> _refreshTokenHandler;
    private readonly Lazy<IAccessTokenHandler> _accessTokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;
    private readonly Lazy<IG32GetUserProfileAvatarUrlFromGoogleHandler> _avatarUrlFromGoogleHandler;
    private readonly IG32Repository _repository;

    public G32VerifyGoogleSignInHandler(
        Lazy<IRefreshTokenHandler> refreshTokenHandler,
        Lazy<IAccessTokenHandler> accessTokenHandler,
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<ISnowflakeIdGenerator> idGenerator,
        Lazy<IG32GetUserProfileAvatarUrlFromGoogleHandler> avatarUrlFromGoogleHandler
    )
    {
        _refreshTokenHandler = refreshTokenHandler;
        _accessTokenHandler = accessTokenHandler;
        _repository = unitOfWork.Value.G32Repository;
        _idGenerator = idGenerator;
        _avatarUrlFromGoogleHandler = avatarUrlFromGoogleHandler;
    }

    public async Task<G32VerifyGoogleSignInResponse> ExecuteAsync(
        G32VerifyGoogleSignInRequest request,
        CancellationToken ct
    )
    {
        // Is user found by email
        var isUserFound = await _repository.IsUserFoundByEmailAsync(request.Email, ct);

        // If user not found by email, then register new user.
        if (!isUserFound)
        {
            // Get user avatar url.
            var userAvatarUrl = _avatarUrlFromGoogleHandler.Value.Execute(request.UserId);

            // Init new user.
            var (newUser, newUserProfile) = CreateNewUser(
                request.UserId,
                request.NickName,
                request.Email,
                userAvatarUrl
            );

            // Generate token id of both refresh and access token.
            var tokenId = _idGenerator.Value.Get().ToString();

            // Create new refresh token.
            var newRefreshToken = InitNewRefreshToken(request.UserId, tokenId, true);

            // Add user and refresh token to database.
            var isAdded = await _repository.AddNewUserAndRefreshTokenToDatabaseAsync(
                newUser,
                newUserProfile,
                newRefreshToken,
                ct
            );

            // Adding fail, please check the error logging in database.
            // Or debug through above step.
            if (!isAdded)
            {
                return new()
                {
                    StatusCode = G32VerifyGoogleSignInResponseStatusCode.DATABASE_ERROR
                };
            }

            // Generate access token.
            var newAccessToken = _accessTokenHandler.Value.Generate(
                claims:
                [
                    new(CommonValues.Claims.TokenIdClaim, tokenId),
                    new(
                        CommonValues.Claims.TokenPurpose.Type,
                        CommonValues.Claims.TokenPurpose.Values.AppUserAccess
                    ),
                    new(CommonValues.Claims.UserIdClaim, request.UserId.ToString())
                ],
                additionalSecondsFromNow: 10 * 60 // 10 minutes
            );

            return new()
            {
                StatusCode = G32VerifyGoogleSignInResponseStatusCode.SUCCESS,
                Body = new() { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Value }
            };
        }
        else
        {
            // Get current user Id.
            var userId = await _repository.GetCurrentUserIdAsync(request.Email, ct);

            // Generate token id of both refresh and access token.
            var tokenId = _idGenerator.Value.Get().ToString();

            // Create new refresh token.
            var newRefreshToken = InitNewRefreshToken(userId, tokenId, true);

            // Add  refresh token to database.
            var isAdded = await _repository.CreateRefreshTokenCommandAsync(newRefreshToken, ct);

            // Adding fail, please check the error logging in database.
            // Or debug through above step.
            if (!isAdded)
            {
                return new()
                {
                    StatusCode = G32VerifyGoogleSignInResponseStatusCode.DATABASE_ERROR
                };
            }

            // Generate access token.
            var newAccessToken = _accessTokenHandler.Value.Generate(
                claims:
                [
                    new(CommonValues.Claims.TokenIdClaim, tokenId),
                    new(
                        CommonValues.Claims.TokenPurpose.Type,
                        CommonValues.Claims.TokenPurpose.Values.AppUserAccess
                    ),
                    new(CommonValues.Claims.UserIdClaim, userId.ToString())
                ],
                additionalSecondsFromNow: 10 * 60 // 10 minutes
            );

            return new()
            {
                StatusCode = G32VerifyGoogleSignInResponseStatusCode.SUCCESS,
                Body = new() { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Value }
            };
        }
    }

    private static (User, UserProfile) CreateNewUser(
        long userId,
        string nickname,
        string email,
        string userAvatarUrl
    )
    {
        var newUser = new User
        {
            Id = userId,
            Email = email,
            UserName = email,
            EmailConfirmed = true
        };

        var newUserProfile = new UserProfile
        {
            UserId = newUser.Id,
            Gender = UserGender.NotSelected,
            NickName = nickname,
            AboutMe = string.Empty,
            AvatarUrl = userAvatarUrl,
            RegisteredAt = DateTime.UtcNow,
            LastActiveAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return (newUser, newUserProfile);
    }

    private UserToken InitNewRefreshToken(long userId, string tokenId, bool isRememberMe)
    {
        return new()
        {
            LoginProvider = tokenId,
            ExpiredAt = isRememberMe
                ? DateTime.UtcNow.AddDays(value: 365)
                : DateTime.UtcNow.AddDays(value: 3),
            UserId = userId,
            Value = _refreshTokenHandler.Value.Generate(length: 15),
            Name = "AppUserRefreshToken"
        };
    }
}
