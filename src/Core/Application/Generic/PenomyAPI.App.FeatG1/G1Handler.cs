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

namespace PenomyAPI.App.FeatG1;

public sealed class G1Handler : IFeatureHandler<G1Request, G1Response>
{
    private readonly IG1Repository _repository;
    private readonly Lazy<IAccessTokenHandler> _accessToken;
    private readonly Lazy<ISendingMailHandler> _mailHandler;

    public G1Handler(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IAccessTokenHandler> accessToken,
        Lazy<ISendingMailHandler> mailHandler
    )
    {
        _repository = unitOfWork.Value.G1Repository;
        _accessToken = accessToken;
        _mailHandler = mailHandler;
    }

    public async Task<G1Response> ExecuteAsync(G1Request request, CancellationToken ct)
    {
        // Does user exist by email.
        var isUserFound = await _repository.IsUserFoundByEmailAsync(email: request.Email, ct: ct);
        // User with email already exists.
        if (isUserFound)
        {
            return new() { StatusCode = G1ResponseStatusCode.USER_EXIST };
        }

        // Generate pre-registration token.
        var preRegistrationToken = _accessToken.Value.Generate(
            [new(CommonValues.Claims.AppUserEmailClaim, request.Email)],
            15 * 60 // 15 minutes
        );

        // Replace the registration link with one that includes the token.
        var body = request.MailTemplate.Replace(
            "{registration_link}",
            GenerateConfirmRegistrationLink(request.RegisterPageLink, preRegistrationToken)
        );

        // Trying to send registration mail.
        var sendingMailResult = await TryToSendMailAsync(request.Email, body, _mailHandler, ct);

        // If seding mail failed [due to some reason].
        if (!sendingMailResult)
        {
            return new() { StatusCode = G1ResponseStatusCode.SENDING_MAIL_FAILED };
        }

        return new() { StatusCode = G1ResponseStatusCode.SUCCESS };
    }

    // MAIN:
    //   - Generate link including pre-registration token.
    //
    //
    // Why creating interpolated string like this?
    // Because this article explains that this is faster from .NET 6
    //
    // - SOURCE: https://btburnett.com/csharp/2021/12/17/string-interpolation-trickery-and-magic-with-csharp-10-and-net-6
    private static string GenerateConfirmRegistrationLink(
        string confirmEmailRegistrationLink,
        string preRegistrationToken
    )
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral(confirmEmailRegistrationLink);
        handler.AppendLiteral("?token=");
        handler.AppendFormatted(preRegistrationToken);

        return handler.ToStringAndClear();
    }

    //  Try to send registration mail.
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
            Subject = "Confim Email Registration",
            Body = mailBody,
        };

        for (var iterator = 0; iterator < 3; iterator++)
        {
            // Trying to send registration mail.
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
