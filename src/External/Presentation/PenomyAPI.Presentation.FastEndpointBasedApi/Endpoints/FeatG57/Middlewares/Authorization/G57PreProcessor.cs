using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG57;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG57.Middlewares;

internal sealed class G57PreProcessor : PreProcessor<G57Request, G57StateBag>
{
    public G57PreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G57Request> context,
        G57StateBag state,
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
