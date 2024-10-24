using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using PenomyAPI.App.Common.Caching;
using PenomyAPI.App.FeatG1.OtherHandlers.VerifyRegistrationToken;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Caching;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Common;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.HttpResponseManager;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG1.VerifyRegistrationToken.Middlewares.Caching;

internal sealed class G1VerifyRegistrationTokenCachingPreProcessor
    : PreProcessor<G1VerifyRegistrationTokenRequest, G1VerifyRegistrationTokenStateBag>
{
    private readonly Lazy<ICacheHandler> _cacheHandler;

    public G1VerifyRegistrationTokenCachingPreProcessor(Lazy<ICacheHandler> cacheHandler)
    {
        _cacheHandler = cacheHandler;
    }

    public override async Task PreProcessAsync(
        IPreProcessorContext<G1VerifyRegistrationTokenRequest> context,
        G1VerifyRegistrationTokenStateBag state,
        CancellationToken ct
    )
    {
        if (context.HttpContext.ResponseStarted())
        {
            return;
        }

        state.CacheKey =
            $"{nameof(G1VerifyRegistrationTokenHttpResponse)}_usertoken_{context.Request.RegistrationToken}";

        // Retrieve from cache.
        var cacheModel =
            await _cacheHandler.Value.GetAsync<G1VerifyRegistrationTokenHttpResponseCaching>(
                state.CacheKey,
                ct
            );

        // Cache value does not exist.
        if (
            !Equals(
                cacheModel,
                AppCacheModel<G1VerifyRegistrationTokenHttpResponseCaching>.NotFound
            )
        )
        {
            await context.HttpContext.Response.SendAsync(
                cacheModel.Value,
                cacheModel.Value.HttpCode,
                cancellation: ct
            );

            context.HttpContext.MarkResponseStart();
        }
    }
}
