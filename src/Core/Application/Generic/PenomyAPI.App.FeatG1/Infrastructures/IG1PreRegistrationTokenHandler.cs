using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG1.Infrastructures;

public interface IG1PreRegistrationTokenHandler
{
    Task<bool> ValidateEmailConfirmationTokenAsync(
        string token,
        CancellationToken cancellationToken
    );

    Task<string> GetEmailFromTokenAsync(string token, CancellationToken cancellationToken);
}
