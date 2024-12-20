namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Password;

public interface IAppPasswordHasher
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string password, string hashedPassword);
}
