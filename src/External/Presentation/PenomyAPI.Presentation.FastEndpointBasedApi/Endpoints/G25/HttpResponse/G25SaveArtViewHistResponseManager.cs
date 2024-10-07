using Microsoft.AspNetCore.Http;
using PenomyAPI.App.G25;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.G25.HttpResponse
{
    public class G25SaveArtViewHistResponseManager
    {
        private static ConcurrentDictionary<
            G25SaveArtViewHistResponseStatusCode,
            Func<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse, G25SaveArtViewHistHttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G25SaveArtViewHistResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G25.{G25SaveArtViewHistResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G25SaveArtViewHistResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"G25.{G25SaveArtViewHistResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<G25SaveArtViewHistRequest, G25SaveArtViewHistResponse, G25SaveArtViewHistHttpResponse> Resolve(
            G25SaveArtViewHistResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
