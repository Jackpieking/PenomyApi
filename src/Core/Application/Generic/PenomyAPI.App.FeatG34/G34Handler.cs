using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PenomyAPI.App.Common;
using PenomyAPI.App.Common.AppConstants;
using PenomyAPI.App.Common.Mail;
using PenomyAPI.App.Common.Tokens;
using PenomyAPI.Domain.RelationalDb.Repositories.Features.Generic;
using PenomyAPI.Domain.RelationalDb.UnitOfWorks;

namespace PenomyAPI.App.FeatG34;

public sealed class G34Handler : IFeatureHandler<G34Request, G34Response>
{
    private readonly IG34Repository _repository;
    private readonly Lazy<ISendingMailHandler> _mailHandler;
    private readonly Lazy<IAccessTokenHandler> _accessToken;

    public G34Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IAccessTokenHandler> accessToken,
        Lazy<ISendingMailHandler> mailHandler
    )
    {
        _repository = unitOfWork.Value.G34Repository;
        _accessToken = accessToken;
        _mailHandler = mailHandler;
    }

    public async Task<G34Response> ExecuteAsync(G34Request request, CancellationToken ct)
    {
        // Does user exist by email.
        var isUserFound = await _repository.IsUserFoundByEmailAsync(email: request.Email, ct: ct);

        // User with email does not exist.
        if (!isUserFound)
        {
            return new() { StatusCode = G34ResponseStatusCode.USER_NOT_EXIST };
        }

        // Generate pre-registration token.
        var preResetPasswordToken = _accessToken.Value.Generate(
            [
                new(
                    CommonValues.Claims.TokenPurposeClaim.ClaimType,
                    CommonValues.Claims.TokenPurposeClaim.ClaimValues.ResetPassword
                ),
                new(CommonValues.Claims.AppUserEmailClaim, request.Email)
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
            Subject = "Reset Password",
            Body = mailBody,
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
}
