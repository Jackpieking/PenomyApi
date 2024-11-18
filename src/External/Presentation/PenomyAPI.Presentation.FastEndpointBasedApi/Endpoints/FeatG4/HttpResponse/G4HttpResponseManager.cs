using Microsoft.AspNetCore.Http;
using PenomyAPI.App.FeatG4;
using PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.DTOs;
using System;
using System.Collections.Concurrent;

namespace PenomyAPI.Presentation.FastEndpointBasedApi.Endpoints.FeatG4.HttpResponse
{
    public static class G4HttpResponseManager
    {
        private static ConcurrentDictionary<
            G4ResponseStatusCode,
            Func<G4Request, G4Response, G4HttpResponse>
        > _dictionary;

        private static void Init()
        {
            _dictionary = new();

            // Add each feature status code with its HttpResponse information.
            _dictionary.TryAdd(
                key: G4ResponseStatusCode.SUCCESS,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G4.{G4ResponseStatusCode.SUCCESS}",
                        HttpCode = StatusCodes.Status200OK,
                        Body = G4ResponseItemDto.MapFromList(response.RecommendedArtworkByCategories)
                    }
            );

            _dictionary.TryAdd(
                key: G4ResponseStatusCode.DATABASE_ERROR,
                value: (_, response) =>
                    new()
                    {
                        AppCode = $"G4.{G4ResponseStatusCode.DATABASE_ERROR}",
                        HttpCode = StatusCodes.Status400BadRequest,
                    }
            );
        }

        internal static Func<G4Request, G4Response, G4HttpResponse> Resolve(
            G4ResponseStatusCode statusCode
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
