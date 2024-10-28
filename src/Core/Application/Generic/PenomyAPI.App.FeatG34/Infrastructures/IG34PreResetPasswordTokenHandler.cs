using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG34.Infrastructures;

public interface IG34PreResetPasswordTokenHandler
{
    Task<string> GetEmailFromTokenAsync(string token, CancellationToken cancellationToken);
}
