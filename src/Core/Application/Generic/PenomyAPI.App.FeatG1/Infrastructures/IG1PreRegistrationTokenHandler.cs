using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG1.Infrastructures;

public interface IG1PreRegistrationTokenHandler
{
    Task<string> GetAsync(string email, CancellationToken ct);

    Task<string> ExtractEmailFromTokenAsync(string token, CancellationToken ct);
}
