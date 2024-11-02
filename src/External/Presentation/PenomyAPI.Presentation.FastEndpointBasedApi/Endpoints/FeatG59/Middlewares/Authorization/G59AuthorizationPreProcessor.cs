using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG59;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG59.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG59.Middlewares.Authorization;

internal sealed class G59AuthorizationPreProcessor : PreProcessor<G59Request, G59StateBag>
{
    public G59AuthorizationPreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G59Request> context,
        G59StateBag state,
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
