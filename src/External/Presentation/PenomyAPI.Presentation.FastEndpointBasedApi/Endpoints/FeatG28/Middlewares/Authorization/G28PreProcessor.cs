using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.FeatG28;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Middlewares;

internal sealed class G28PreProcessor : PreProcessor<G28Request, G28StateBag>
{
    public G28PreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G28Request> context,
        G28StateBag state,
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
