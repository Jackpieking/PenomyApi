using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.IdGenerator.Snowflake;
using PenomyAPI.App.Common.Mail;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Domain.RelationalDb.Entities.UserIdentity;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34;

public sealed class G34Handler : IFeatureHandler<G34Request, G34Response>
{
    private readonly IG34Repository _repository;
    private readonly Lazy<ISendingMailHandler> _mailHandler;
    private readonly Lazy<IAccessTokenHandler> _accessToken;
    private readonly Lazy<ISnowflakeIdGenerator> _idGenerator;

    public G34Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IAccessTokenHandler> accessToken,
        Lazy<ISendingMailHandler> mailHandler,
        Lazy<ISnowflakeIdGenerator> idGenerator
    )
    {
        _repository = unitOfWork.Value.G34Repository;
        _accessToken = accessToken;
        _mailHandler = mailHandler;
        _idGenerator = idGenerator;
    }

    public async Task<G34Response> ExecuteAsync(G34Request request, CancellationToken ct)
    {
        // Get user id by email.
        var foundUserId = await _repository.GetUserIdByEmailAsync(request.Email, ct);

        // User with email does not exist.
        if (foundUserId == default)
        {
            return new() { StatusCode = G34ResponseStatusCode.USER_NOT_EXIST };
        }

        // Generate reset password token metadata.
        var preResetPasswordTokenMetadata = InitNewResetPasswordMetadataToken(
            foundUserId,
            _idGenerator.Value.Get().ToString()
        );

        // Persist reset password token metadata to database
        var dbResult = await _repository.SaveResetPasswordTokenMetadataAsync(
            preResetPasswordTokenMetadata,
            ct
        );

        // If database error.
        if (!dbResult)
        {
            return new() { StatusCode = G34ResponseStatusCode.DATABASE_ERROR };
        }

        // Generate reset password token.
        var preResetPasswordToken = _accessToken.Value.Generate(
            [
                new(CommonValues.Claims.TokenIdClaim, preResetPasswordTokenMetadata.LoginProvider),
                new(
                    CommonValues.Claims.TokenPurposeClaim.ClaimType,
                    CommonValues.Claims.TokenPurposeClaim.ClaimValues.ResetPassword
                ),
                new(CommonValues.Claims.UserIdClaim, foundUserId.ToString())
            ],
            15 * 60 // 15 minutes
        );

        // Replace the reset password link with one that includes the token.
        var body = request.MailTemplate.Replace(
            "{forgot_password_link}",
            GenerateResetPasswordLink(request.CurrentResetPasswordLink, preResetPasswordToken)
        );

        // Trying to send mail.
        var sendingMailResult = await TryToSendMailAsync(request.Email, body, _mailHandler, ct);

        // If seding mail failed [due to some reason].
        if (!sendingMailResult)
        {
            return new() { StatusCode = G34ResponseStatusCode.SENDING_MAIL_FAILED };
        }

        return new() { StatusCode = G34ResponseStatusCode.SUCCESS };
    }

    // MAIN:
    //   - Generate link including password reset token.
    //
    //
    // Why creating interpolated string like this?
    // Because this article explains that this is faster from .NET 6
    //
    // - SOURCE: https://btburnett.com/csharp/2021/12/17/string-interpolation-trickery-and-magic-with-csharp-10-and-net-6
    private static string GenerateResetPasswordLink(
        string resetPasswordLink,
        string resetPasswordToken
    )
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral(resetPasswordLink);
        handler.AppendLiteral("?token=");
        handler.AppendFormatted(resetPasswordToken);

        return handler.ToStringAndClear();
    }

    //  Try to sending mail.
    //  With retry if failed.
    private static async Task<bool> TryToSendMailAsync(
        string email,
        string mailBody,
        Lazy<ISendingMailHandler> mailHandler,
        CancellationToken ct
    )
    {
        var mailContent = new AppMailContent
        {
            To = email,
            Subject = "Thay đổi mật khẩu",
            Body = mailBody
        };

        for (var iterator = 0; iterator < 3; iterator++)
        {
            // Trying to send mail.
            var sendingResult = await mailHandler.Value.SendAsync(mailContent, ct);

            // If seding mail successfully,  end the task
            // Otherwise, retry sending mail
            if (sendingResult)
            {
                return true;
            }
        }

        return false;
    }

    private static UserToken InitNewResetPasswordMetadataToken(long userId, string tokenId)
    {
        return new()
        {
            LoginProvider = tokenId,
            ExpiredAt = DateTime.UtcNow.AddMinutes(15),
            UserId = userId,
            Value = string.Empty,
            Name = "AppUserPreResetPasswordToken"
        };
    }
}
