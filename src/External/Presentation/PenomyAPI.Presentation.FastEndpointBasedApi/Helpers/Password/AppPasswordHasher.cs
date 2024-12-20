using System;
using System.Security.Cryptography;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Helpers.Password;

public class AppPasswordHasher : IAppPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;
    private const string HashPasswordSplitter = "-";
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}{HashPasswordSplitter}{Convert.ToHexString(salt)}";
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(HashPasswordSplitter);

        var oldHash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(oldHash, newHash);
    }
}
