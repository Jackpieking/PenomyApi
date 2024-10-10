using System.Threading.Tasks;
using PenomyAPI.App.FeatG3.Infrastructures;

namespace PenomyAPI.Infra.FeatG3;

public class FeatG3MailHandler : IFeatG3MailHandler
{
    public Task<bool> SendMailAsync()
    {
        return Task.FromResult(true);
    }
}
