using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.DTOs;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatSM23.Middlewares.Authorization;

internal sealed class SM23AuthorizationPreProcessor : PreProcessor<SM23RequestDto, SM23StateBag>
{
    public SM23AuthorizationPreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<SM23RequestDto> context,
        SM23StateBag state,
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
            state.AppRequest.SetUserId("0");
        } else
        // Save found user id to state bag.
        state.AppRequest.SetUserId(userId);
        return Task.CompletedTask;
    }
}
