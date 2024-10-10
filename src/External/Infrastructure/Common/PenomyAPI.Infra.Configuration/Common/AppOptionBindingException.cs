using System;

namespace PenomyAPI.Infra.Configuration.Common;

public sealed class AppOptionBindingException : Exception
{
    public AppOptionBindingException() { }

    public AppOptionBindingException(string message)
        : base(message) { }
}
