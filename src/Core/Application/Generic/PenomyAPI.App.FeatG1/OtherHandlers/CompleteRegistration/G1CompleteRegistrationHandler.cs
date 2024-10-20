using PenomyAPI.App.Common;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.FeatG1.Infrastructures;
using PenomyAPI.Domain.RelationalDb.Entities.Generic;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        // TODO
        _repository = unitOfWork.Value.G1Repository;
    }

    public async Task<G1CompleteRegistrationResponse> ExecuteAsync(
        G1CompleteRegistrationRequest request,
        CancellationToken ct
    )
    {
        // Extract email from token.
        var result = await _preRegistrationTokenHandler.Value.GetEmailFromTokenAsync(
            request.PreRegistrationToken,
            ct
        );

        // Invalid pre-registration token
        // - maybe users insert them,
        // - not from penomy website but by
        //      calling api directly with their token =)).
        if (!result.IsSuccess)
        {
            return new()
            {
                StatusCode = G1CompleteRegistrationResponseStatusCode.INVALID_PRE_REGISTRATION_TOKEN
            };
        }

        // Generate bundle for new user.
        var newUserEmail = result.Value;

        var (newUser, newuserProfile) = CreateNewUser(newUserEmail, request.ConfirmedNickName);

        // Add bundle to database.
        var isAdded = await _repository.AddNewUserToDatabaseAsync(
            newUser,
            newuserProfile,
            request.Password,
            ct
        );

        // Adding user fail, please check the error logging in database.
        // Or debug through above step.
        if (isAdded)
        {
            return new() { StatusCode = G1CompleteRegistrationResponseStatusCode.DATABASE_ERROR };
        }

        return new()
        {
            NewUserEmail = newUserEmail,
            StatusCode = G1CompleteRegistrationResponseStatusCode.SUCCESS
        };
    }

    private (User, UserProfile) CreateNewUser(string email, string nickname)
    {
        var newUser = new User
        {
            Id = _snowflakeIdGenerator.Value.Get(),
            Email = email,
            UserName = email
        };

        // TODO: Add avatar url
        var datetimeUtcNow = DateTime.UtcNow;
        var newUserProfile = UserProfile.NewProfile(newUser.Id, nickname, "None");
        newUserProfile.RegisteredAt = datetimeUtcNow;
        newUserProfile.LastActiveAt = datetimeUtcNow;
        newUserProfile.UpdatedAt = datetimeUtcNow;

        return (newUser, newUserProfile);
    }
}
