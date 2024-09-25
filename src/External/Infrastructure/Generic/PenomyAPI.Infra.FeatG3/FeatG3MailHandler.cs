using PenomyAPI.App.FeatG3.Infrastructures;

namespace PenomyAPI.Infra.FeatG3;

public class FeatG3MailHandler : IFeatG3MailHandler
{
    public async Task<bool> SendMailAsync()
    {
        return true;
    }
}
