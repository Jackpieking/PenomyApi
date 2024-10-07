using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG8;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG8.HttpResponse
{
    public class G8ResponseManager
    {
        private static ConcurrentDictionary<
            G8ResponseStatusCode,
            Func<G8Request, G8Response, G8HttpResponse>
        > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G8ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G8.{G8ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                    }
            );

            _dictionary.TryAdd(
                key: G8ResponseStatusCode.FAILED,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G8.{G8ResponseStatusCode.FAILED}",
                        HttpCode = StatusCodes.Status500InternalServerError,
                    }
            );
            _dictionary.TryAdd(
                key: G8ResponseStatusCode.INVALID_REQUEST,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G8.{G8ResponseStatusCode.INVALID_REQUEST}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
            _dictionary.TryAdd(
                key: G8ResponseStatusCode.NOT_FOUND,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G8.{G8ResponseStatusCode.NOT_FOUND}",
                        HttpCode = StatusCodes.Status404NotFound,
                    }
            );
        }

        internal static Func<G8Request, G8Response, G8HttpResponse> Resolve(
            G8ResponseStatusCode statusCode
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
