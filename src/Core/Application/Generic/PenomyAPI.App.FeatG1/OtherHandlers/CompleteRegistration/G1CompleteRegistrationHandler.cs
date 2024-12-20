using System;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG1.OtherHandlers.CompleteRegistration;

public sealed class G1CompleteRegistrationHandler
    : IFeatureHandler<G1CompleteRegistrationRequest, G1CompleteRegistrationResponse>
{
    private readonly Lazy<IG1PreRegistrationTokenHandler> _preRegistrationTokenHandler;
    private readonly Lazy<ISnowflakeIdGenerator> _snowflakeIdGenerator;
    private readonly IG1Repository _repository;

    public G1CompleteRegistrationHandler(
        Lazy<IG1PreRegistrationTokenHandler> preRegistrationTokenHandler,
        Lazy<ISnowflakeIdGenerator> snowflakeIdGenerator,
        Lazy<IUnitOfWork> unitOfWork
    )
    {
        _preRegistrationTokenHandler = preRegistrationTokenHandler;
        _snowflakeIdGenerator = snowflakeIdGenerator;
        _repository = unitOfWork.Value.G1Repository;
    }

    public async Task<G1CompleteRegistrationResponse> ExecuteAsync(
        G1CompleteRegistrationRequest request,
        CancellationToken ct
    )
    {
        // Extract email from token.
        var email = await _preRegistrationTokenHandler.Value.GetEmailFromTokenAsync(
            request.PreRegistrationToken,
            ct
        );

        // Invalid pre-registration token
        // - maybe users insert them,
        // - not from penomy website but by
        //      calling api directly with their token =)).
        if (string.IsNullOrWhiteSpace(email))
        {
            return new() { StatusCode = G1CompleteRegistrationResponseStatusCode.INVALID_TOKEN };
        }

        // Is user found by email.
        var isUserFound = await _repository.IsUserFoundByEmailAsync(email, ct);

        // Email already registered
        if (isUserFound)
        {
            return new() { StatusCode = G1CompleteRegistrationResponseStatusCode.USER_EXIST };
        }

        // Pre init new user profile.
        var newUser = PreInitUserProfile(email);

        // Validate user password
        var isPasswordValid = await _repository.ValidatePasswordAsync(newUser, request.Password);

        // User password is invalid
        if (!isPasswordValid)
        {
            return new() { StatusCode = G1CompleteRegistrationResponseStatusCode.PASSWORD_INVALID };
        }

        // Complete init user profile process.
        var newuserProfile = CreateNewUser(newUser, request.ConfirmedNickName);

        // Add bundle to database.
        var isAdded = await _repository.AddNewUserToDatabaseAsync(
            newUser,
            newuserProfile,
            request.Password,
            ct
        );

        // Adding user fail, please check the error logging in database.
        // Or debug through above step.
        if (!isAdded)
        {
            return new() { StatusCode = G1CompleteRegistrationResponseStatusCode.DATABASE_ERROR };
        }

        return new()
        {
            NewUserEmail = email,
            StatusCode = G1CompleteRegistrationResponseStatusCode.SUCCESS
        };
    }

    private User PreInitUserProfile(string email)
    {
        return new()
        {
            Id = _snowflakeIdGenerator.Value.Get(),
            Email = email,
            UserName = email,
            EmailConfirmed = true,
        };
    }

    private static UserProfile CreateNewUser(User newUser, string nickname)
    {
        var newUserProfile = new UserProfile
        {
            UserId = newUser.Id,
            Gender = UserGender.NotSelected,
            NickName = nickname,
            AboutMe = string.Empty,
            AvatarUrl = CommonValues.Others.DefaultUserAvaterUrl,
            RegisteredAt = DateTime.UtcNow,
            LastActiveAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UpdateNickNameAt = DateTime.UtcNow,
        };

        return newUserProfile;
    }
}
