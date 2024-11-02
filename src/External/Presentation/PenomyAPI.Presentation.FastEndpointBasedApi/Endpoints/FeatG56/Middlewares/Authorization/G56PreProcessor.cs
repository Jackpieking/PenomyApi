using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG56;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG56.Middlewares;

internal sealed class G56PreProcessor : PreProcessor<G56Request, G56StateBag>
{
    public G56PreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G56Request> context,
        G56StateBag state,
        CancellationToken ct
    )
    {
        // Bypass if response has started.
        if (context.HttpContext.ResponseStarted())
        {
            return Task.CompletedTask;
        }

        // Get user id.
        var userId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (userId is null)
        {
            userId = "-1";
        }
        // Save found user id to state bag.
        state.AppRequest.SetUserId(userId);

        return Task.CompletedTask;
    }
}
