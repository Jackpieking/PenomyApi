using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using PenomyAPI.App.FeatG10;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.DTOs;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G10.HttpResponse;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG10.Middlewares.Authorization;

internal sealed class G10AuthorizationPreProcessor : PreProcessor<G10Request, G10StateBag>
{
    public G10AuthorizationPreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G10Request> context,
        G10StateBag state,
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
