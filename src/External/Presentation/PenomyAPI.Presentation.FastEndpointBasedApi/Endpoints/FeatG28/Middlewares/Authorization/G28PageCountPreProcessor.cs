using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.FeatG28.PageCount;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.Common;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG28.PageCount.Middlewares;

internal sealed class G28PageCountPreProcessor : PreProcessor<G28PageCountRequest, G28PageCountStateBag>
{
    public G28PageCountPreProcessor() { }

    public override Task PreProcessAsync(
        IPreProcessorContext<G28PageCountRequest> context,
        G28PageCountStateBag state,
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
        state.PageCountRequest.SetUserId(userId);
        return Task.CompletedTask;
    }
}
