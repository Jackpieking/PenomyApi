using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25.OtherHandlers.NumberArtViewed;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse
{
    public class G25CountArtViewedResponseManager
    {
        private static ConcurrentDictionary<
            G25CountArtViewedResponseStatusCode,
            Func<G25CountArtViewedRequest, G25CountArtViewedResponse, G25CountArtViewedHttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G25CountArtViewedResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G25.{G25CountArtViewedResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G25CountArtViewedResponseStatusCode.EMPTY,
                value: (_, response) => new()
                {
                    AppCode = $"G25.{G25CountArtViewedResponseStatusCode.EMPTY}",
                    HttpCode = StatusCodes.Status404NotFound,
                });
        }

        internal static Func<G25CountArtViewedRequest, G25CountArtViewedResponse, G25CountArtViewedHttpResponse> Resolve(
            G25CountArtViewedResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
