using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG52;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG52.HttpResponse
{
    public static class G52HttpResponseManager
    {
        private static ConcurrentDictionary<
            G52ResponseStatusCode,
            Func<G52Request, G52Response, G52HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G52ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G52.{G52ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: G52ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"G52.{G52ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<G52Request, G52Response, G52HttpResponse> Resolve(
            G52ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
