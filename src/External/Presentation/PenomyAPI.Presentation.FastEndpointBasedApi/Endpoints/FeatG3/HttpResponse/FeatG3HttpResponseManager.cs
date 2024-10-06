using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG3;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG3.HttpResponse
{
    public static class G3HttpResponseManager
    {
        private static ConcurrentDictionary<
            FeatG3ResponseStatusCode,
            Func<FeatG3Request, FeatG3Response, FeatG3HttpResponse>> _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: FeatG3ResponseStatusCode.SUCCESS,
                value: (_, response) => new()
                {
                    AppCode = $"G3.{FeatG3ResponseStatusCode.SUCCESS}",
                    HttpCode = StatusCodes.Status200OK,
                });

            _dictionary.TryAdd(
                key: FeatG3ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) => new()
                {
                    AppCode = $"G3.{FeatG3ResponseStatusCode.DATABASE_ERROR}",
                    HttpCode = StatusCodes.Status400BadRequest,
                });
        }

        internal static Func<FeatG3Request, FeatG3Response, FeatG3HttpResponse> Resolve(
            FeatG3ResponseStatusCode statusCode)
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
