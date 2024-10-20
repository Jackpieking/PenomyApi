using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using PenomyAPI.App.Common.Tokens;

namespace PenomyAPI.Identity.AppAuthenticator;

public sealed class RefreshTokenHandler : IRefreshTokenHandler
{
    public string Generate(int length)
    {
        const string Chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz!@#$%^&*+=";

        var handler = new DefaultInterpolatedStringHandler();

        for (int order = 0; order < length; order++)
        {
            handler.AppendFormatted(Chars[RandomNumberGenerator.GetInt32(Chars.Length)]);
        }

        return handler.ToString();
    }
}
