namespace PenomyAPI.App.FeatG3.Infrastructures;

public interface IFeatG3MailHandler
{
    Task<bool> SendMailAsync();
}
