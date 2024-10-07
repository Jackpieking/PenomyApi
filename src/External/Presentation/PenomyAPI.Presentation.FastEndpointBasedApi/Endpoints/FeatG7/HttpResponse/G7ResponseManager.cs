using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG7;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG7.HttpResponse
{
    public class G7ResponseManager
    {
        private static ConcurrentDictionary<
            G7ResponseStatusCode,
            Func<G7Request, G7Response, G7HttpResponse>
        > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G7ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G7.{G7ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                    }
            );

            _dictionary.TryAdd(
                key: G7ResponseStatusCode.FAILED,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G7.{G7ResponseStatusCode.FAILED}",
                        HttpCode = StatusCodes.Status500InternalServerError,
                    }
            );
            _dictionary.TryAdd(
                key: G7ResponseStatusCode.INVALID_REQUEST,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G7.{G7ResponseStatusCode.INVALID_REQUEST}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
            _dictionary.TryAdd(
                key: G7ResponseStatusCode.NOT_FOUND,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G7.{G7ResponseStatusCode.NOT_FOUND}",
                        HttpCode = StatusCodes.Status404NotFound,
                    }
            );
        }

        internal static Func<G7Request, G7Response, G7HttpResponse> Resolve(
            G7ResponseStatusCode statusCode
        )
        {
            if (Equals(objA: _dictionary, objB: default))
            {
                Init();
            }

            return _dictionary[statusCode];
        }
    }
}
