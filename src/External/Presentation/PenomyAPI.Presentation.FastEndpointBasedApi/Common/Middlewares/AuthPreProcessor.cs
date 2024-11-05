using FastEndpoints;
using PenomyAPI.Presentation.FastEndpointBasedApi.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Common.Middlewares;

internal sealed class AuthPreProcessor<T> : PreProcessor<T, StateBag> where T : class
{
    public override async Task PreProcessAsync(
        IPreProcessorContext<T> context,
        StateBag state,
        CancellationToken ct
    )
    {
        #region PreValidateAccessToken
        // Extract and convert access token expire time.
        var tokenExpireTime = JwtHelper.ExtractUtcTimeFromToken(context.HttpContext);

        // Is token expired.
        if (tokenExpireTime < DateTime.UtcNow)
        {
            //await SendResponseAsync(
            //    G48ResponseStatusCode.FORBIDDEN,
            //    state.AppRequest,
            //    context.HttpContext,
            //    ct
            //);

            return;
        }

        #endregion
        // Get user id
        var userId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!long.TryParse(userId, out var id))
        {
            //await SendResponseAsync(
            //    G48ResponseStatusCode.UN_AUTHORIZED,
            //    state.AppRequest,
            //    context.HttpContext,
            //    ct
            //);

            return;
        }

        // Save found user id to state bag.
        state.AppRequest.UserId = id;
    }

    ///// <summary>
    /////     Send response extension method.
    ///// </summary>
    ///// <param name="statusCode">
    /////     The app status code.
    ///// </param>
    ///// <param name="appRequest">
    /////     The app request.
    ///// </param>
    ///// <param name="context">
    /////     The http context.
    ///// </param>
    ///// <param name="ct">
    /////     The cancellation token used to propagate
    /////     notification that operations should be canceled.
    ///// </param>
    ///// <returns>
    /////     Empty
    ///// </returns>
    //private static Task SendResponseAsync(
    //    ResponseStatusCode statusCode,
    //    AuthRequest appRequest,
    //    HttpContext context,
    //    CancellationToken ct
    //)
    //{
    //    var httpResponse = G48ResponseManager
    //        .Resolve(statusCode)
    //        .Invoke(appRequest, new() { StatusCode = statusCode });

    //    return context.Response.SendAsync(httpResponse, httpResponse.HttpCode, cancellation: ct);
    //}
}
