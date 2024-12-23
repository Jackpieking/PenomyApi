﻿using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.App.FeatG1.Infrastructures;

public interface IG1PreRegistrationTokenHandler
{
    Task<string> GetEmailFromTokenAsync(string token, CancellationToken cancellationToken);
}
